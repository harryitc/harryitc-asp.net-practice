using System.Collections.Generic;
using System.Threading.Tasks;
using FlowerShop.Models;

namespace FlowerShop.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> FilterProducts(string? name, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? isDiscount);

        // Phương thức quản lý hình ảnh sản phẩm
        Task AddProductImageAsync(ProductImage productImage);
        Task DeleteProductImageAsync(int imageId);
        Task DeleteAllProductImagesAsync(int productId);
        Task<IEnumerable<ProductImage>> GetProductImagesAsync(int productId);
    }
}
