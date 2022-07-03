using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

using OrderCart.Repository;
using OrderCart.Models;

namespace OrderCart.Controllers
{
    public class ProductController : Controller
    {
        IConfiguration configuration;
        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.GetResponse("products/");
                response.EnsureSuccessStatusCode();
                List<Models.Product> products = response.Content.ReadAsAsync<List<Models.Product>>().Result;
                return View(products);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            
        }
        // GET: ProductController
        
       


        // GET: ProductController/Details/5
        public ActionResult Details(Models.Product product)
        {
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(Models.Product product)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddSeconds(15);
            Response.Cookies.Append("Alert", product.Title + " has been added to cart", option);
            
            return RedirectToAction("Details", "Order", product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }



        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
