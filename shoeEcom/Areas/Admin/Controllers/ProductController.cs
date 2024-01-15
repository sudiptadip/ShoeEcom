using Ecom.DataAccess;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace shoeEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUniteOfWork uniteOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _uniteOfWork = uniteOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> Products = _uniteOfWork.Product.GetAll(includeProperty: "Category,ProductImages");
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

                string wwwRootPath = _webHostEnvironment.WebRootPath;


                if(files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\product\product-" + obj.Id.ToString();
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using(var fileStreem = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStreem);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = obj.Id,
                        };

                        if(obj.ProductImages == null)
                        {
                            obj.ProductImages = new List<ProductImage>();
                        }

                        obj.ProductImages.Add(productImage);
                        _uniteOfWork.ProductImage.Add(productImage);
                        _uniteOfWork.Save();
                    }
                }                
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