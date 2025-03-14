using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlowerShop.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

public class UnsplashService : IImageService
{
    private readonly HttpClient _httpClient;
    private readonly string _unsplashAccessKey;

    public UnsplashService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _unsplashAccessKey = configuration["Unsplash:AccessKey"];
    }

    public async Task<List<string>> GetUnsplashImagesAsync(string query, int count)
    {
        string url = $"https://api.unsplash.com/photos/random?query={query}&count={count}&client_id={_unsplashAccessKey}";

        var response = await _httpClient.GetStringAsync(url);
        var images = JArray.Parse(response);

        return images.Select(img => img["urls"]["small"].ToString()).ToList();
    }
}



//using System.Text.Json;

//namespace FlowerShop.Services
//{
//    public class ImageService
//    {
//        private readonly HttpClient _httpClient;
//        private const string UnsplashApiUrl = "https://api.unsplash.com/photos/random?client_id=YOUR_ACCESS_KEY&count=10";

//        public ImageService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<List<string>> GetImagesFromUnsplashAsync()
//        {
//            var response = await _httpClient.GetStringAsync(UnsplashApiUrl);
//            var images = JsonSerializer.Deserialize<List<UnsplashImage>>(response);
//            return images.Select(img => img.Urls.Small).ToList();
//        }
//    }

//    public class UnsplashImage
//    {
//        public UnsplashUrls Urls { get; set; }
//    }

//    public class UnsplashUrls
//    {
//        public string Small { get; set; }
//    }

//}
