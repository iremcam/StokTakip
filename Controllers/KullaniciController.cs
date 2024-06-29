using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StokTakip.Data;
using StokTakip.Models;
using StokTakip.Services;
using System.Configuration;

namespace StokTakip.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly DataConnection dbConnection;
        private readonly KullaniciServisi kullaniciServisi;

        public KullaniciController(DataConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            this.kullaniciServisi = new KullaniciServisi(dbConnection);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Kullanici());
        }

        [HttpPost]
        public IActionResult Giris(Kullanici kullanici)
        {
            try
            {
                
                if (kullaniciServisi.GirisYap(kullanici.KullaniciAdi, kullanici.Sifre))
                {
                    return RedirectToAction("Index", "Urunler"); 
                }
                else
                {
                    ViewBag.ErrorMessage = "Kullanıcı adı veya şifre yanlış.";
                    return View("Index", kullanici); 
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Giriş yapılırken bir hata oluştu: " + ex.Message;
                return View("Index", kullanici);
            }
            finally
            {
                dbConnection.Close(); 
            }
        }

        
        [HttpPost]
        public IActionResult Kayit(Kullanici kullanici)
        {
            try
            {
                kullaniciServisi.KullaniciEkle(kullanici);
                ViewBag.SuccessMessage = "Kullanıcı başarıyla kaydedildi.";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Kullanıcı eklenirken bir hata oluştu: " + ex.Message;
                return View("Index", kullanici); 
            }
            finally
            {
                dbConnection.Close(); 
            }
        }
    }
}
