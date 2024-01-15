using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Ecom.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NuGet.Packaging;
using shoeEcom.Models;
using System.Diagnostics;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace shoeEcom.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUniteOfWork _uniteOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUniteOfWork uniteOfWork, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _uniteOfWork = uniteOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            IndexVM vm = new()
            {
                MensShoe = _uniteOfWork.Product.GetAll(u => u.Gender == "men", includeProperty: "Category,ProductImages").Take(8),
                TrendingShoe = _uniteOfWork.Product.GetAll(u => u.Category.Name == "C", includeProperty: "Category,ProductImages").Take(4),
                WomensShoe = _uniteOfWork.Product.GetAll(u => u.Gender == "women", includeProperty: "Category,ProductImages").Take(7),
                Boots = _uniteOfWork.Product.GetAll(u => u.Category.Name == "B", includeProperty: "Category,ProductImages").Take(4),
            };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var product = _uniteOfWork.Product.Get(u => u.Id == id, includeProperty: "Category,ProductImages");

            DetailsVM vm = new()
            {
                ShoppingCart = new()
                {
                    Product = product
                },
                OtherOption = _uniteOfWork.Product.GetAll(u => u.Gender == "women", includeProperty: "Category,ProductImages").Take(7)
            };

            if(product != null)
            {
                return View(vm);
            }
            else{
                return NotFound();
            }
                       
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(DetailsVM obj)
        {
            string userId = _userManager.GetUserId(User);
            obj.ShoppingCart.Count = 1;
            obj.ShoppingCart.ApplicationUserId = userId;

            _uniteOfWork.ShoppingCart.Add(obj.ShoppingCart);
            _uniteOfWork.Save();
            TempData["Success"] = "Add To Cart Successfully Done";
            return RedirectToAction("index");
        }

        public IActionResult AllProduct(int[] selectedCategories, string[] gender, string sort, int pageNumber)
        {
            ViewBag.Category = _uniteOfWork.Category.GetAll();
            IEnumerable<Product> allProducts;
            PaginatedList<Product> paginatedList;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            int pageSize = 9;


            var predicate = PredicateBuilder.New<Product>(p => true);

            // Category and Gender conditions
            if (selectedCategories.Count() > 0)
            {
                predicate = predicate.And(p => selectedCategories.Contains(p.CategoryId));
            }

            if (gender.Count() > 0)
            {
                predicate = predicate.And(p => gender.Contains(p.Gender));
            }

            // Sorting
            switch (sort)
            {
                case "asc":
                    predicate = predicate.And(p => true);
                    allProducts = _uniteOfWork.Product.GetAll(predicate, includeProperty: "ProductImages").OrderBy(p => p.Price).ToList();
                    paginatedList = new PaginatedList<Product>(allProducts, pageNumber, pageSize);
                    return View(paginatedList);
                case "desc":
                    predicate = predicate.And(p => true);
                    allProducts = _uniteOfWork.Product.GetAll(predicate, includeProperty: "ProductImages").OrderByDescending(p => p.Price).ToList();
                    paginatedList = new PaginatedList<Product>(allProducts, pageNumber, pageSize);
                    return View(paginatedList);
                default:
                    break;
            }

            allProducts = _uniteOfWork.Product.GetAll(predicate, includeProperty: "ProductImages");
            paginatedList = new PaginatedList<Product>(allProducts, pageNumber, pageSize);

            return View(paginatedList);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return RedirectToAction("NotFound");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotFound()
        {
            return View();
        }
    }
}