using BookStore.DataAccess.IRepository;
using BookStore.Models.Models;
using BookStore.Models.ViewModels;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _iuow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
                _iuow = unitOfWork;
                _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> companies = _iuow.Company.GetAll().ToList();
            return View(companies);
        }
        public IActionResult UpsertC(int? id)
        {

            Company company = new();
            if (id == null || id == 0)
            {
                // Create company
                return View(company);
            }
            else
            {
                // Edit company
                Company companyExist = _iuow.Company.GetFirstOrDefault(p => p.Id == id);
                if (companyExist == null)
                {
                    return NotFound();
                }
                return View(companyExist);
            }
        }

        [HttpPost]
        public IActionResult UpsertC(Company company)
        {

            if (ModelState.IsValid)
            {

                if (company.Id == 0)
                {
                    _iuow.Company.Add(company);
                }
                else
                {
                    _iuow.Company.Update(company);
                }
                _iuow.Save();
                TempData["Success"] = " Product Company successfully.";
                return RedirectToAction("Index");
            }
            else
            {                
                TempData["Error"] = "Error occured due to invalid model state.";
                return View();
            }
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _iuow.Company.GetAll().ToList();
            return Json(new { data = companies });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _iuow.Company.GetFirstOrDefault(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _iuow.Company.Remove(companyToBeDeleted);
            _iuow.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
