using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}
