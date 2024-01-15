using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace shoeEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        public UserController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> users = _uniteOfWork.ApplicationUser.GetAll().OrderByDescending(u => u.CreatedOn);
            List<ApplicationUser> userList = users.ToList();
            return View(userList);
        }

        public IActionResult UserDetails(string userId) 
        {
            ViewBag.User = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId); 
            IEnumerable<OrderItem> orderItems = _uniteOfWork.OrderItem.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product.ProductImages,Product,OrderAddress");
            return View(orderItems);
        }
    }
}
