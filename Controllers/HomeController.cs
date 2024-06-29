using Microsoft.AspNetCore.Mvc;
using StokTakip.Data;
using StokTakip.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace StokTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataConnection _db;
        public HomeController(ILogger<HomeController> logger,DataConnection db)
        {
            _logger = logger;
            _db=db;
        }

        public IActionResult Index()
        {
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
