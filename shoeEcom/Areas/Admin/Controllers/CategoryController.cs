using Ecom.DataAccess;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace shoeEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        public CategoryController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categorys = _uniteOfWork.Category.GetAll();
            return View(categorys);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.Category.Add(obj);
                _uniteOfWork.Save();
                ViewBag.Success = "Successfully Category Created";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int id)
        {
            Category category = _uniteOfWork.Category.Get(u => u.Id == id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(Category obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.Category.Update(obj);
                _uniteOfWork.Save();
                ViewBag.Success = "Successfully Category Update";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Category category = _uniteOfWork.Category.Get(u => u.Id == id);

            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            _uniteOfWork.Category.Remove(category);
            _uniteOfWork.Save();
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}