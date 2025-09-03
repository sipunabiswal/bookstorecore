using BookStoreCore.Data;
using BookStoreCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreCore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
                _db = db;
        }
        public IActionResult Index()
        {
            List<Category> categories= _db.Categories.ToList();

            return View(categories);
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display order can't be same as name.");
            }
            if (!string.IsNullOrEmpty(category.Name) && category.Name.ToLower().Contains("test"))
            {
                ModelState.AddModelError("", "Test is an invalid input.");
            }

            if (!ModelState.IsValid)
            {
                TempData["Failure"] = "Category creation Failed due to invalid data.";
                return View(category);
            }

            _db.Categories.Add(category);
            _db.SaveChanges();
            TempData["Success"] = "Category created successfullly.";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult EditCategory(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {

            
            if (!ModelState.IsValid)
            {
                TempData["Failure"] = "Category update Failed.";
                return View(category);
            }
            _db.Categories.Update(category);
            _db.SaveChanges();
            TempData["Success"] = "Category updated successfullly.";
            return RedirectToAction("Index", "Category");
        }
        public IActionResult DeleteCategory(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryPost(int? id)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category==null)
            {
                TempData["Failure"] = "Category deleted Failed.";
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["Failure"] = "Category deleted Failed.";
                return View(category);
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["Success"] = "Category deleted successfullly.";
            return RedirectToAction("Index", "Category");
        }
    }
}
