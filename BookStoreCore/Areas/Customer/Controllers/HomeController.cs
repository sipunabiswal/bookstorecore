using System.Diagnostics;
using BookStore.DataAccess.IRepository;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreCore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            Product cartObj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category");
            
            return View(cartObj);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
