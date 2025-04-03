using FlowerShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TextTemplating;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.ProductImages).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductImages)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // ⭐ Hàm lọc sản phẩm ⭐
        public async Task<IEnumerable<Product>> FilterProducts(string? name, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? isDiscount)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                string name_khong_dau = VietnameseHelper.RemoveDiacritics(name);
                query = query.Where(p => p.Name_khongdau.Contains(name_khong_dau));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (isDiscount.HasValue && isDiscount.Value)
            {
                query = query.Where(p => p.Discount > 0);
            }

            return query.ToList();
        }

        // Phương thức quản lý hình ảnh sản phẩm
        public async Task AddProductImageAsync(ProductImage productImage)
        {
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductImageAsync(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllProductImagesAsync(int productId)
        {
            var images = await _context.ProductImages.Where(pi => pi.ProductId == productId).ToListAsync();
            if (images.Any())
            {
                _context.ProductImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesAsync(int productId)
        {
            return await _context.ProductImages.Where(pi => pi.ProductId == productId).ToListAsync();
        }
    }

}
