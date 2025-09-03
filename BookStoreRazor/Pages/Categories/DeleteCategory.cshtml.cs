using BookStoreRazor.Data;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazor.Pages.Categories
{
    [BindProperties]
    public class DeleteCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category? Category { get; set; }
        public DeleteCategoryModel(ApplicationDbContext db)
        {
                _db = db;   
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category = _db.Categories.FirstOrDefault(Category => Category.Id == id);
            }
        }
        public IActionResult OnPost(int? id)
        {
            
                Category = _db.Categories.FirstOrDefault(Category => Category.Id == id);
                if (Category == null)
                {
                    return NotFound();
                }
                _db.Categories.Remove(Category);
                _db.SaveChanges();
                TempData["success"] = "Category deleted successfully";
                return RedirectToPage("Index");
            
            
        }
    }
}
