namespace FlowerShop.Services
{
    public interface IImageService
    {
        Task<List<string>> GetUnsplashImagesAsync(string query, int count);
    }
}
