using Ecom.DataAccess.Migrations;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ShoppingCartController(IUniteOfWork uniteOfWork, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _uniteOfWork = uniteOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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
            checkoutVM.User = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);

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
            TempData["success"] = "Order placed successfully! Thank you for shopping with us.";
            return RedirectToAction("Manage", "Account", new { area = "Identity" });
        }


        [HttpPost]
        public void UploadProfile(IFormFile img, string userId)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            ApplicationUser applicationUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);

            if (img != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\profile");

                if (!string.IsNullOrEmpty(applicationUser.ProfileImgUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, applicationUser.ProfileImgUrl.TrimStart('\\'));

                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    img.CopyTo(fileStream);
                }

                applicationUser.ProfileImgUrl = @"\images\profile\" + fileName;
                _uniteOfWork.ApplicationUser.Update(applicationUser);
                _uniteOfWork.Save();
            }
        }

    }
}
