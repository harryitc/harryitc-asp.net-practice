using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Models;
using FlowerShop.Repository;
using FlowerShop.Services;

namespace FlowerShop.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly ImageUploadService _imageUploadService;
        private readonly IImageService _imageService;

        public ProductImageController(
            IProductImageRepository productImageRepository,
            IProductRepository productRepository,
            ImageUploadService imageUploadService,
            IImageService imageService
            )
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _imageUploadService = imageUploadService;
            _imageService = imageService;
        }

        // GET: ProductImage
        public async Task<IActionResult> Index(int? productId)
        {
            ViewBag.Products = await _productRepository.GetAllAsync();

            if (productId.HasValue)
            {
                var product = await _productRepository.GetByIdAsync(productId.Value);
                if (product == null)
                    return NotFound();

                ViewBag.SelectedProduct = product;
                var productImages = await _productImageRepository.GetByProductIdAsync(productId.Value);
                return View(productImages);
            }
            else
            {
                // Nhóm hình ảnh theo sản phẩm
                var allImages = await _productImageRepository.GetAllAsync();
                var groupedImages = allImages
                    .GroupBy(img => img.ProductId)
                    .Select(group => new ProductImagesGroup
                    {
                        ProductId = group.Key,
                        Product = group.First().Product,
                        Images = group.ToList(),
                        Count = group.Count()
                    })
                    .ToList();

                return View("IndexGrouped", groupedImages);
            }
        }

        // GET: ProductImage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var productImage = await _productImageRepository.GetByIdAsync(id.Value);
            if (productImage == null)
                return NotFound();

            return View(productImage);
        }

        // GET: ProductImage/Create
        public async Task<IActionResult> Create(int? productId)
        {
            var products = await _productRepository.GetAllAsync();
            ViewData["ProductId"] = new SelectList(products, "Id", "Name", productId);

            if (productId.HasValue)
            {
                var product = await _productRepository.GetByIdAsync(productId.Value);
                if (product != null)
                {
                    ViewData["SelectedProduct"] = product;
                }
            }

            var images = await _imageService.GetUnsplashImagesAsync("flowers", 12);
            ViewData["ImageList"] = images;

            return View(new ProductImage { ProductId = productId ?? 0 });
        }

        // POST: ProductImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,ProductId,IsMainImage")] ProductImage productImage,
        IFormFile? ImageFile
        )
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng chọn upload file
                if (ImageFile != null)
                {
                    productImage.ImageUrl = await _imageUploadService.Save(ImageFile);
                }

                // Nếu đặt làm hình ảnh chính, hủy các hình ảnh chính khác của sản phẩm này
                if (productImage.IsMainImage)
                {
                    var productImages = await _productImageRepository.GetByProductIdAsync(productImage.ProductId);
                    foreach (var img in productImages)
                    {
                        if (img.IsMainImage)
                        {
                            img.IsMainImage = false;
                            await _productImageRepository.UpdateAsync(img);
                        }
                    }
                }

                await _productImageRepository.AddAsync(productImage);

                // Chuyển hướng về trang chi tiết sản phẩm nếu có productId
                if (productImage.ProductId > 0)
                {
                    return RedirectToAction(nameof(Index), new { productId = productImage.ProductId });
                }

                return RedirectToAction(nameof(Index));
            }

            var products = await _productRepository.GetAllAsync();
            ViewData["ProductId"] = new SelectList(products, "Id", "Name", productImage.ProductId);

            if (productImage.ProductId > 0)
            {
                var product = await _productRepository.GetByIdAsync(productImage.ProductId);
                if (product != null)
                {
                    ViewData["SelectedProduct"] = product;
                }
            }

            var images = await _imageService.GetUnsplashImagesAsync("flowers", 12);
            ViewData["ImageList"] = images;

            return View(productImage);
        }

        // GET: ProductImage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var productImage = await _productImageRepository.GetByIdAsync(id.Value);
            if (productImage == null)
                return NotFound();

            var products = await _productRepository.GetAllAsync();
            ViewData["ProductId"] = new SelectList(products, "Id", "Name", productImage.ProductId);

            var images = await _imageService.GetUnsplashImagesAsync("flowers", 5);
            ViewData["ImageList"] = images;

            return View(productImage);
        }

        // POST: ProductImage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageUrl,ProductId")] ProductImage productImage,
        IFormFile? ImageFile
        )
        {
            if (id != productImage.Id)
                return NotFound();

            if (ModelState.IsValid)
            {

                // Nếu người dùng chọn upload file
                if (ImageFile != null)
                {
                    productImage.ImageUrl = await _imageUploadService.Save(ImageFile);
                }


                if (!await _productImageRepository.ExistsAsync(id))
                    return NotFound();

                await _productImageRepository.UpdateAsync(productImage);
                return RedirectToAction(nameof(Index));
            }

            var products = await _productRepository.GetAllAsync();
            ViewData["ProductId"] = new SelectList(products, "Id", "Name", productImage.ProductId);
            return View(productImage);
        }

        // GET: ProductImage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var productImage = await _productImageRepository.GetByIdAsync(id.Value);
            if (productImage == null)
                return NotFound();

            return View(productImage);
        }

        // POST: ProductImage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productImageRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: ProductImage/SetMainImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMainImage(int id, bool isMain)
        {
            try
            {
                var image = await _productImageRepository.GetByIdAsync(id);
                if (image == null)
                    return NotFound();

                // Nếu đặt làm hình ảnh chính, hủy các hình ảnh chính khác của sản phẩm này
                if (isMain)
                {
                    var productImages = await _productImageRepository.GetByProductIdAsync(image.ProductId);
                    foreach (var img in productImages)
                    {
                        if (img.Id != id && img.IsMainImage)
                        {
                            img.IsMainImage = false;
                            await _productImageRepository.UpdateAsync(img);
                        }
                    }
                }

                // Cập nhật trạng thái hình ảnh hiện tại
                image.IsMainImage = isMain;
                await _productImageRepository.UpdateAsync(image);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
