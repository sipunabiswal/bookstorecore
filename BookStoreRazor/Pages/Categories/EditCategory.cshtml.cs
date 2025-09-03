using BookStoreRazor.Data;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazor.Pages.Categories
{
    [BindProperties]
    public class EditCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public EditCategoryModel(ApplicationDbContext db)
        {
                _db = db;
        }
        public void OnGet(int? id)
        {
            if (id!=null && id!=0)
            {
                Category = _db.Categories.FirstOrDefault(Category => Category.Id == id);
            }
            
        }
        public IActionResult OnPost() 
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
