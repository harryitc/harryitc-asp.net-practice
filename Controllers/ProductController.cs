using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FlowerShop.Controllers;
using FlowerShop.Repository;
using FlowerShop.Services;
using Microsoft.AspNetCore.Authorization;
using FlowerShop.Models;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IImageService _imageService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly ImageUploadService _imageUploadService;

    public ProductController(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IWebHostEnvironment webHostEnvironment,
        IImageService imageService,
        ImageUploadService imageUploadService
        )
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _webHostEnvironment = webHostEnvironment;
        _imageService = imageService;
        _imageUploadService = imageUploadService;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        ViewData["Categories"] = await _categoryRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        return View(products);
    }

    // ⭐ Action lọc sản phẩm ⭐
    public async Task<IActionResult> Filter(string? name, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? isDiscount)
    {
        ViewData["Categories"] = await _categoryRepository.GetAllAsync();
        var filteredProducts = await _productRepository.FilterProducts(name, categoryId, minPrice, maxPrice, isDiscount);
        return View("Index", filteredProducts);
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null) return NotFound();

        return View(product);
    }


    // GET: Product/Create
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

        var images = await _imageService.GetUnsplashImagesAsync("flowers", 5);
        ViewData["ImageList"] = images;

        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,OriginialPrice,Discount,Promotion,ImageUrl,Description,CategoryId")] Product product,
        IFormFile? ImageFile, List<IFormFile>? AdditionalImages)
    {
        if (ModelState.IsValid)
        {
            // Nếu người dùng chọn upload file
            if (ImageFile != null)
            {
                product.ImageUrl = await _imageUploadService.Save(ImageFile);
            }

            product.Name_khongdau = VietnameseHelper.RemoveDiacritics(product.Name);

            // Lưu sản phẩm trước để có Id
            await _productRepository.AddAsync(product);

            // Xử lý các hình ảnh bổ sung
            if (AdditionalImages != null && AdditionalImages.Count > 0)
            {
                foreach (var image in AdditionalImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imageUrl = await _imageUploadService.Save(image);
                        var productImage = new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = imageUrl
                        };

                        await _productRepository.AddProductImageAsync(productImage);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // GET: Product/Edit/5
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null) return NotFound();

        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);

        var images = await _imageService.GetUnsplashImagesAsync("flowers", 5);
        ViewData["ImageList"] = images;

        return View(product);
    }

    //public async Task<IActionResult> Edit(int? id)
    //{
    //    if (id == null) return NotFound();

    //    var product = await _productRepository.GetByIdAsync(id.Value);
    //    if (product == null) return NotFound();

    //    // Lấy danh sách ảnh từ Unsplash
    //    var imageList = await _imageService.GetImagesFromUnsplashAsync();

    //    var viewModel = new ProductViewModel
    //    {
    //        Id = product.Id,
    //        Name = product.Name,
    //        Price = product.Price,
    //        Description = product.Description,
    //        CategoryId = product.CategoryId,
    //        ImageUrl = product.ImageUrl,
    //        AvailableImages = imageList
    //    };

    //    ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
    //    return View(viewModel);
    //}



    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,OriginialPrice,Discount,Promotion,ImageUrl,Description,CategoryId")] Product product,
        IFormFile? ImageFile, List<IFormFile>? AdditionalImages, bool? DeleteExistingImages
        )
    {
        if (id != product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            // Lấy sản phẩm hiện tại để giữ lại các thông tin không được cập nhật
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            // Cập nhật các trường
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.OriginialPrice = product.OriginialPrice;
            existingProduct.Discount = product.Discount;
            existingProduct.Promotion = product.Promotion;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Name_khongdau = VietnameseHelper.RemoveDiacritics(product.Name);

            // Nếu người dùng chọn upload file
            if (ImageFile != null)
            {
                existingProduct.ImageUrl = await _imageUploadService.Save(ImageFile);
            }
            else
            {
                // Giữ lại URL hình ảnh cũ nếu không có hình mới
                existingProduct.ImageUrl = product.ImageUrl;
            }

            try
            {
                // Xóa tất cả hình ảnh hiện tại nếu được yêu cầu
                if (DeleteExistingImages == true)
                {
                    await _productRepository.DeleteAllProductImagesAsync(id);
                }

                // Xử lý các hình ảnh bổ sung
                if (AdditionalImages != null && AdditionalImages.Count > 0)
                {
                    foreach (var image in AdditionalImages)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var imageUrl = await _imageUploadService.Save(image);
                            var productImage = new ProductImage
                            {
                                ProductId = id,
                                ImageUrl = imageUrl
                            };

                            await _productRepository.AddProductImageAsync(productImage);
                        }
                    }
                }

                // Cập nhật sản phẩm
                await _productRepository.UpdateAsync(existingProduct);

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(product.Id)) return NotFound();
                else throw;
            }
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // GET: Product/Delete/5
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null) return NotFound();

        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ProductExists(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null;
    }
}
