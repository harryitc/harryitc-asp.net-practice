using System;

namespace FlowerShop.Models.Payment
{
    public class MoMoRequest
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string RequestId { get; set; }
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
        public string ExtraData { get; set; }
        public string RequestType { get; set; } = "captureMoMoWallet";
        public string Signature { get; set; }
    }
}
