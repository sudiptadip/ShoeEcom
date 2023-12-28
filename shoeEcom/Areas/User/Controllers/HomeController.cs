using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Microsoft.AspNetCore.Mvc;
using shoeEcom.Models;
using System.Diagnostics;

namespace shoeEcom.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUniteOfWork _uniteOfWork;

        public HomeController(ILogger<HomeController> logger, IUniteOfWork uniteOfWork)
        {
            _logger = logger;
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index(int pageNumber)
        {
            IEnumerable<Category> products = _uniteOfWork.Category.GetAll().Take(2);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
