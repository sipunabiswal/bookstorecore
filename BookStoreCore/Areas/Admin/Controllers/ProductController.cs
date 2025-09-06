using BookStore.DataAccess.IRepository;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreCore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _iuow;
        public ProductController(IUnitOfWork unitOfWork )
        {
            _iuow = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> products = _iuow.Product.GetAll().ToList();
            return View(products);
        }
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {

            if (ModelState.IsValid)
            {
                _iuow.Product.Add(product);
                _iuow.Save();
                TempData["Success"] = " Product created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error occured due to invalid model state.";
                return View();
            }
        }
        public IActionResult EditProduct(int? id )
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
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _iuow.Product.Update(product);
                _iuow.Save();
                TempData["Success"] = " Product updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error occured due to invalid model state.";
                return View(product);
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
    }
}
