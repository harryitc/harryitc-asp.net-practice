﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FlowerShop.Controllers;
using FlowerShop.Repository;
using FlowerShop.Services;

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
    public async Task<IActionResult> Create()
    {

        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToAction("Unauthorized", "Auth");
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

        var images = await _imageService.GetUnsplashImagesAsync("flowers", 5);
        ViewData["ImageList"] = images;

        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,ImageUrl,Description,CategoryId")] Product product,
        IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            // Nếu người dùng chọn upload file
            if (ImageFile != null)
            {
                product.ImageUrl = await _imageUploadService.Save(ImageFile);
            }

            await _productRepository.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // GET: Product/Edit/5
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImageUrl,Description,CategoryId")] Product product,
        IFormFile? ImageFile
        )
    {
        if (id != product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            // Nếu người dùng chọn upload file
            if (ImageFile != null)
            {
                product.ImageUrl = await _imageUploadService.Save(ImageFile);
            }
            try
            {
                await _productRepository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(product.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // GET: Product/Delete/5
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

    [HttpGet("GetUnsplashImages")]
    public async Task<IActionResult> GetUnsplashImages()
    {
        var images = await _imageService.GetUnsplashImagesAsync("flowers", 5);
        return Ok(images);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return Json(new { success = false, message = "Không có file nào được chọn." });
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var imageUrl = "/uploads/" + fileName;
        return Json(new { success = true, imageUrl });
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay
        using (var fileStream = new FileStream(savePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        return "/images/" + image.FileName;
    }

}
