﻿using Microsoft.AspNetCore.Mvc;
using ProductSample.Models.ViewModel;
using ProductSample.Services;

namespace ProductSample.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService,
            ISupplierService supplierService, ICategoryService categoryService)

        {
            _productService = productService;
            _supplierService = supplierService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _productService.GetAllAsync();
            return View(res);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
        // var res= await _productService.AddAsync(productViewModel);
         var res= await _productService.AddWithSPAsync(productViewModel);
            if(res==ProductResult.SuccessFull) return RedirectToAction(nameof(Index));
            return RedirectToRoute("/Home/Error");
        }

        public async Task<IActionResult> Edit(int id)
        {

            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();

            var product = await _productService.GetByIdAsync(id);
            return View(product);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {


            //validation
            await _productService.UpdateAsync(productViewModel);
             return RedirectToAction(nameof(Index));

        }

         public async Task<IActionResult> Delete(int id)
        {


            //validation
            await _productService.DeleteByIdAsync(id);
            return RedirectToAction(nameof(Index));

        }

    }
}