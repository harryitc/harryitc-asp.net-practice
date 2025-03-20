using FlowerShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Repository
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetAllAsync();
        Task<ProductImage> GetByIdAsync(int id);
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task AddAsync(ProductImage productImage);
        Task UpdateAsync(ProductImage productImage);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}
