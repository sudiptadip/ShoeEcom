using Ecom.DataAccess;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace shoeEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        public ProductController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> Products = _uniteOfWork.Product.GetAll();
            return View(Products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _uniteOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            ViewBag.CategoryList = categoryList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.Product.Add(obj);
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int id)
        {
            Product Product = _uniteOfWork.Product.Get(u => u.Id == id);
            return View(Product);
        }
        [HttpPost]
        public IActionResult Update(Product obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.Product.Update(obj);
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Product Product = _uniteOfWork.Product.Get(u => u.Id == id);

            if (Product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _uniteOfWork.Product.Remove(Product);
            _uniteOfWork.Save();

            return Ok(new { message = "Product deleted successfully" });
        }
    }
}