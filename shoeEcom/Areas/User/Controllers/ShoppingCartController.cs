using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
