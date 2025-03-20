using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Models;
using FlowerShop.Repository;

namespace FlowerShop.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;

        public ProductImageController(IProductImageRepository productImageRepository, IProductRepository productRepository)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
        }

        // GET: ProductImage
        public async Task<IActionResult> Index()
        {
            var images = await _productImageRepository.GetAllAsync();
            return View(images);
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
        public async Task<IActionResult> Create()
        {
            var products = await _productRepository.GetAllAsync();
            ViewData["ProductId"] = new SelectList(products, "Id", "Name");
            return View();
        }

        // POST: ProductImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,ProductId")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                await _productImageRepository.AddAsync(productImage);
                return RedirectToAction(nameof(Index));
            }

            var products = await _productRepository.GetAllAsync();
            ViewData["ProductId"] = new SelectList(products, "Id", "Name", productImage.ProductId);
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
            return View(productImage);
        }

        // POST: ProductImage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageUrl,ProductId")] ProductImage productImage)
        {
            if (id != productImage.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
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
    }
}
