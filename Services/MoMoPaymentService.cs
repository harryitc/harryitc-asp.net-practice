using FlowerShop.Models.Payment;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.Services
{
    public class MoMoPaymentService
    {
        private readonly MoMoConfig _momoConfig;
        private readonly HttpClient _httpClient;

        public MoMoPaymentService(IOptions<MoMoConfig> momoConfig, HttpClient httpClient)
        {
            _momoConfig = momoConfig.Value;
            _httpClient = httpClient;
        }

        public async Task<MoMoResponse> CreatePaymentAsync(string orderId, long amount, string orderInfo)
        {
            try
            {
                var requestId = DateTime.UtcNow.Ticks.ToString();
                var rawHash = $"partnerCode={_momoConfig.PartnerCode}" +
                              $"&accessKey={_momoConfig.AccessKey}" +
                              $"&requestId={requestId}" +
                              $"&amount={amount}" +
                              $"&orderId={orderId}" +
                              $"&orderInfo={orderInfo}" +
                              $"&returnUrl={_momoConfig.ReturnUrl}" +
                              $"&notifyUrl={_momoConfig.IpnUrl}" +
                              $"&extraData=";

                var signature = ComputeHmacSha256(rawHash, _momoConfig.SecretKey);

                var request = new MoMoRequest
                {
                    PartnerCode = _momoConfig.PartnerCode,
                    AccessKey = _momoConfig.AccessKey,
                    RequestId = requestId,
                    Amount = amount.ToString(),
                    OrderId = orderId,
                    OrderInfo = orderInfo,
                    ReturnUrl = _momoConfig.ReturnUrl,
                    NotifyUrl = _momoConfig.IpnUrl,
                    ExtraData = "",
                    Signature = signature
                };

                var jsonRequest = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_momoConfig.PaymentUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var momoResponse = JsonConvert.DeserializeObject<MoMoResponse>(responseContent);
                    return momoResponse;
                }

                return new MoMoResponse
                {
                    ErrorCode = -1,
                    Message = "Failed to connect to MoMo payment gateway"
                };
            }
            catch (Exception ex)
            {
                return new MoMoResponse
                {
                    ErrorCode = -1,
                    Message = ex.Message
                };
            }
        }

        public bool ValidateIPNSignature(MoMoIPNRequest ipnRequest)
        {
            var rawHash = $"partnerCode={ipnRequest.PartnerCode}" +
                          $"&accessKey={ipnRequest.AccessKey}" +
                          $"&requestId={ipnRequest.RequestId}" +
                          $"&amount={ipnRequest.Amount}" +
                          $"&orderId={ipnRequest.OrderId}" +
                          $"&orderInfo={ipnRequest.OrderInfo}" +
                          $"&orderType={ipnRequest.OrderType}" +
                          $"&transId={ipnRequest.TransId}" +
                          $"&errorCode={ipnRequest.ErrorCode}" +
                          $"&message={ipnRequest.Message}" +
                          $"&localMessage={ipnRequest.LocalMessage}" +
                          $"&payType={ipnRequest.PayType}" +
                          $"&responseTime={ipnRequest.ResponseTime}" +
                          $"&extraData={ipnRequest.ExtraData}";

            var signature = ComputeHmacSha256(rawHash, _momoConfig.SecretKey);
            return signature.Equals(ipnRequest.Signature);
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
