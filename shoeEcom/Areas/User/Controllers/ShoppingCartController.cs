using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Ecom.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace shoeEcom.Areas.User.Controllers
{
    [Area("User")]
    public class ShoppingCartController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public ShoppingCartController(IUniteOfWork uniteOfWork, UserManager<IdentityUser> userManager)
        {
            _uniteOfWork = uniteOfWork;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            IEnumerable<ShoppingCart> cartItems = _uniteOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product.Category,Product.ProductImages");

            double Subtotals = 0;
            double Discount = 0;

            foreach (var cart in cartItems) 
            { 
                Subtotals += cart.Product.Price;
                Discount += cart.Product.Price - cart.Product.DiscountPrice;
            }

            ViewBag.Subtotals = Subtotals;
            ViewBag.Discount = Discount;


            return View(cartItems);
        }

        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            ShoppingCart cart = _uniteOfWork.ShoppingCart.Get(u => u.Id == id);

            if (cart == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            _uniteOfWork.ShoppingCart.Remove(cart);
            _uniteOfWork.Save();
            return Ok(new { message = "Cart deleted successfully" });
        }

        [Authorize]
        public IActionResult Checkout()
        {
            string userId = _userManager.GetUserId(User);
            CheckoutVM checkoutVM = new CheckoutVM();
            checkoutVM.Carts = (List<ShoppingCart>)_uniteOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product,Product.ProductImages");

            double Subtotals = 0;
            double Discount = 0;
            foreach (var cart in checkoutVM.Carts)
            {
                Subtotals += cart.Product.Price;
                Discount += cart.Product.Price - cart.Product.DiscountPrice;
            }

            ViewBag.Subtotals = Subtotals;
            ViewBag.Discount = Discount;
            return View(checkoutVM);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(CheckoutVM checkoutVM)
        {
            string userId = _userManager.GetUserId(User);
            checkoutVM.OrderAddress.ApplicationUserId = userId;

            _uniteOfWork.OrderAddress.Add(checkoutVM.OrderAddress);
            _uniteOfWork.Save();
            TempData["OrderAddressId"] = checkoutVM.OrderAddress.Id;
            return RedirectToAction("Payment");
        }

        [Authorize]
        public IActionResult Payment()
        {
            if (TempData.TryGetValue("OrderAddressId", out var orderAddressId))
            {

                string userId = _userManager.GetUserId(User);
                var carts = _uniteOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product,Product.ProductImages");
                double Subtotals = 0;
                double Discount = 0;
                foreach (var cart in carts)
                {
                    Subtotals += cart.Product.Price;
                    Discount += cart.Product.Price - cart.Product.DiscountPrice;
                }

                ViewBag.Subtotals = Subtotals;
                ViewBag.Discount = Discount;
                ViewBag.OrderAddressId = orderAddressId;

                return View();
            }

            return RedirectToAction("Checkout", "ShoppingCart");

        }

        [HttpPost]
        public IActionResult Payment(OrderItem obj)
        {
            int orderAddressId = obj.OrderAddressId;
            string paymentType = obj.PaymentType;

            string userId = _userManager.GetUserId(User);
            var carts = _uniteOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product,Product.ProductImages");

            foreach (var item in carts)
            {
                OrderItem orderItem = new()
                {
                    Price = item.Product.Price,
                    DiscountPrice = item.Product.DiscountPrice,
                    Size = item.Size,
                    OrderStatus = "Success",
                    PaymentType = paymentType,
                    OrderTime = DateTime.Now,
                    ProductId = item.ProductId,
                    ApplicationUserId = userId,
                    OrderAddressId = orderAddressId
                };
                _uniteOfWork.OrderItem.Add(orderItem);
                _uniteOfWork.Save();
            }

            _uniteOfWork.ShoppingCart.RemoveRange(carts);
            _uniteOfWork.Save();

            return RedirectToAction("Index", "Home");
        }

    }
}
