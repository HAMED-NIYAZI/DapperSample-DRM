using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
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
            var res = await _productService.AddWithSPAsync(productViewModel);
            if (res == ProductResult.SuccessFull) return RedirectToAction(nameof(Index));
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

        public  IActionResult BulkInsert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BulkInsert(IFormFile excelFile)
        {
            var products = new List<ProductViewModel>();

            using (var memoryStrem = new MemoryStream())
            {
                excelFile.CopyTo(memoryStrem);
                using (var reader = ExcelReaderFactory.CreateReader(memoryStrem))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString()?.ToLower() == "ProductName".ToLower())
                            {
                                continue;
                            }
                            var product = new ProductViewModel
                            {
                                ProductName = reader[0].ToString(),
                                CategoryID = Convert.ToInt32(reader[1]),
                                SupplierID = Convert.ToInt32(reader[2]),
                                UnitPrice = Convert.ToDouble(reader[3])
                            };

                            products.Add(product);
                        }
                    } while (reader.NextResult());
                }
                if (products.Count > 0)
                {
                    await _productService.BulkAddAsync(products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
