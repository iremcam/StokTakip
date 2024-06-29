using Microsoft.AspNetCore.Mvc;
using StokTakip.Data;
using StokTakip.Models;
using StokTakip.Services;
using System.Data.SqlClient;
using static StokTakip.Services.UrunServisi;

namespace StokTakip.Controllers
{
    public class UrunlerController : Controller
    {
        private readonly DataConnection _dataConnection;

        private readonly UrunServisi _urunServisi;
        public UrunlerController(DataConnection dataConnection,UrunServisi urunServisi)
        {
            _dataConnection = dataConnection;
            _urunServisi= urunServisi;
        }

       
        public IActionResult Detail(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return NotFound();
            }

            ProductDetailViewModel viewModel = _urunServisi.GetProductDetailViewModel(productCode);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Urunler urunler)
        {
            
                _urunServisi.UrunEkle(urunler);
                ViewBag.SuccessMessage = "Ürün başarıyla kaydedildi.";
                return RedirectToAction("Index"); 
            

            
        }


        [HttpPost]
        public IActionResult Update(ProductDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                _urunServisi.UpdateProduct(model.ProductCode, model.Name, model.Color, model.Length, model.Quantity);
                return RedirectToAction("Index"); 
            }

            return View();
        }
        public IActionResult Delete(string productCode)
        {
            if (ModelState.IsValid)
            {
                _urunServisi.UrunSil(productCode);
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("Hatalı işlem");
            }
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<ProductDetailViewModel> products = _urunServisi.GetProductList();
            return View(products);
        }
        [HttpGet]
        public IActionResult GetProductStocks()
        {
            var products = _urunServisi.GetProductStocks();
            return Json(products);
        }
        [HttpGet]
        public IActionResult SearchByProductCode(string productCode)
        {
            var product = _urunServisi.GetProductDetailViewModel(productCode);
            return Json(product);
        }
    }
}
