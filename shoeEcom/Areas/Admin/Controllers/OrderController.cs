using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shoeEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        public OrderController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<OrderItem> orderItems = _uniteOfWork.OrderItem.GetAll(includeProperty: "Product,Product.ProductImages,OrderAddress");
            return View(orderItems);
        }

        [HttpPost]
        public IActionResult Index(string orderStatus, int id)
        {
            OrderItem orderItem = _uniteOfWork.OrderItem.Get(u => u.Id == id);
            orderItem.OrderStatus = orderStatus;
            _uniteOfWork.OrderItem.Update(orderItem);
            _uniteOfWork.Save();
            TempData["success"] = "Order status successfully update";
            return RedirectToAction("Index");
        }
    }
}
