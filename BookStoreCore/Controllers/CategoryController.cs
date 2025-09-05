
using BookStore.DataAccess.Data;
using BookStore.DataAccess.IRepository;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreCore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository  _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }
        public IActionResult Index()
        {
            List<Category> categories= _categoryRepo.GetAll().ToList();

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

            _categoryRepo.Add(category);
            _categoryRepo.Save();
            TempData["Success"] = "Category created successfullly.";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult EditCategory(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            Category? category = _categoryRepo.GetFirstOrDefault(c => c.Id == Id);
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
            _categoryRepo.Update(category);
            _categoryRepo.Save();
            TempData["Success"] = "Category updated successfullly.";
            return RedirectToAction("Index", "Category");
        }
        public IActionResult DeleteCategory(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            Category? category = _categoryRepo.GetFirstOrDefault(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryPost(int? id)
        {
            Category? category = _categoryRepo.GetFirstOrDefault(c => c.Id == id);
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
            _categoryRepo.Remove(category);
            _categoryRepo.Save();
            TempData["Success"] = "Category deleted successfullly.";
            return RedirectToAction("Index", "Category");
        }
    }
}
