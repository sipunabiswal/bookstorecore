using BookStore.DataAccess.IRepository;
using BookStore.Models.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreCore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
                _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShappingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
            };
            foreach (var cart in ShoppingCartVM.ShappingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Summary() { 
            return View();
        }



        public IActionResult Plus(int cartId)
        {
            var existingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            existingCart.Count += 1;
            _unitOfWork.ShoppingCart.Update(existingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var existingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (existingCart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(existingCart);
            }
            else
            {
                existingCart.Count -= 1;
                _unitOfWork.ShoppingCart.Update(existingCart);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var existingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(existingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods
        private double GetPriceBasedOnQuantity(ShoppingCart cart)
        {
            if (cart.Count <= 50)
            {
                return cart.Product.Price;
            }
            else
            {
                if (cart.Count <= 100)
                {
                    return cart.Product.Price50;
                }
                else
                {
                    return cart.Product.Price100;
                }
            }
        }
        #endregion
    }
}
