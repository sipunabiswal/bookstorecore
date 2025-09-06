using BookStore.DataAccess.IRepository;
using BookStore.Models.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _iuow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment )
        {
            _iuow = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _iuow.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _iuow.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            //ViewBag.CategoryList = categoryList;
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = categoryList
            };
            if (id == null || id == 0)
            {
                // Create product
                return View(productVM);
            }
            else
            {
                // Edit product
                productVM.Product = _iuow.Product.GetFirstOrDefault(p => p.Id == id, includeProperties:"Category");
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM pvm, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if (file !=null)
                {
                    string rootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(rootPath, @"images\products");
                    if (!string.IsNullOrEmpty(pvm.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(rootPath, pvm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    pvm.Product.ImageUrl = @"\images\products\" + fileName;
                }

                if (pvm.Product.Id == 0)
                {
                    _iuow.Product.Add(pvm.Product);
                }
                else
                {
                    _iuow.Product.Update(pvm.Product);
                }
                _iuow.Save();
                TempData["Success"] = " Product created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                pvm.CategoryList = _iuow.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
                TempData["Error"] = "Error occured due to invalid model state.";
                return View();
            }
        }

        public IActionResult DeleteProduct(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? product = _iuow.Product.GetFirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("DeleteProduct")]
        public IActionResult DeleteProductSelected(int? id)
        {
            Product? product = _iuow.Product.GetFirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _iuow.Product.Remove(product);
            _iuow.Save();
            TempData["Success"] = " Product deleted successfully.";
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _iuow.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _iuow.Product.GetFirstOrDefault(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _iuow.Product.Remove(productToBeDeleted);
            _iuow.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
