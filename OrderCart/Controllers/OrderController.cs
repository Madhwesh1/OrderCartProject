using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrderCart.Controllers
{
    public class OrderController : Controller
    {

        private ApiContext _Context;

        public OrderController(ApiContext context)
        {
            _Context = context;
        }

        // GET: OrderController
        public ActionResult Index()
        {
            List<Models.CartProduct> lstCartObject = new List<Models.CartProduct>();
            Models.Cart cartObj;
            try
            {
                using (_Context)
                {
                    List<Models.Cart> cartlist = _Context.CartObject.ToList();
                    
                    if (cartlist.Count()>0)
                    {
                        var proId = _Context.CartProjObject.ToList().GroupBy(s => s.Id).First();
                        if (cartlist.First().Product.Count==0)
                        {
                            cartlist.First().Product.AddRange(proId);

                        }
                        
                        cartObj = cartlist.First();
                    }
                    else
                    {
                        cartObj = null;
                    }
                }

            }
            catch (Exception)
            {
                cartObj = null;
                return View("Error");
            }

                return View(cartObj);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(Models.Product product)
        {
            using (_Context)
            {
                var cartItem = _Context.CartObject.ToList();
               
                if (cartItem != null && cartItem.Count()>0)
                {
                    
                    var proId = _Context.CartProjObject.Where(s => s.Id == product.Id).FirstOrDefault();
                    var cartobj = _Context.CartObject.First();
                    if (proId != null)
                    {
                        proId.Quantity++;
                        proId.Price = proId.Price + product.Price;
                        _Context.CartProjObject.Update(proId);
                        cartobj.price += product.Price;
                        _Context.CartObject.Update(cartobj);
                        _Context.SaveChanges();

                    }
                    else
                    {
                        Models.CartProduct intobj = new Models.CartProduct();
                        intobj.Id = product.Id;
                        intobj.Title = product.Title;
                        intobj.Quantity = 1;
                        intobj.Price = product.Price;
                        _Context.CartProjObject.Add(intobj);
                        cartobj.price += product.Price;
                        cartobj.Product = new List<Models.CartProduct>();
                        cartobj.Product.Add(intobj);
                        _Context.CartObject.Update(cartobj);
                        _Context.SaveChanges();

                    }
                  
        
                }
                else
                {
                    Models.CartProduct intobj = new Models.CartProduct();
                    intobj.Id = product.Id;
                    intobj.Title = product.Title;
                    intobj.Quantity = 1;
                    intobj.Price = product.Price;

                    Models.Cart iniCart = new Models.Cart();
                    iniCart.price = product.Price;
                    iniCart.orderStatus = "Draft";
                    iniCart.cartId = 1;
                    iniCart.Product = new List<Models.CartProduct>();
                    iniCart.Product.Add(intobj);
                    _Context.CartProjObject.Add(intobj);
                    _Context.CartObject.Add(iniCart);
                   
                    _Context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Product");


        }

        // GET: OrderController/Create
      
        public ActionResult Create(Models.Cart cart)
        {
            try
            {
                using (_Context)
                {
                   
                    foreach (var item in _Context.CartObject)
                    {
                        _Context.CartObject.Remove(item);
                        
                        _Context.SaveChanges();
                    }                   
                    foreach (Models.CartProduct item in _Context.CartProjObject)
                    {
                        _Context.CartProjObject.Remove(item);
                        _Context.SaveChanges();
                    }
                    _Context.SaveChanges();                    
                 
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://ordercart.requestcatcher.com/test");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<Models.Cart>("Cart", cart);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddSeconds(15);
                        Response.Cookies.Append("Alert", "Order is SuccessFul . Thank you for shopping", option);
                        return RedirectToAction("Index", "Product");

                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            }
            catch (Exception)
            {

                return View("Error");
            }
           

            return View("Error");
        }
        

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
 
        public ActionResult Delete(Models.CartProduct product)
        {
            try
            {
                using (_Context)
                {                    
                    var cartItem = _Context.CartObject.ToList();
                    var proId = _Context.CartProjObject.Where(s => s.Id == product.Id).First();
                    var cartobj = _Context.CartObject.First();
                    if (proId.Quantity>1)
                    {
                        proId.Quantity--;
                        proId.Price = proId.Price - product.Price;
                        _Context.CartProjObject.Update(proId);
                        cartobj.price -= product.Price;
                        _Context.CartObject.Update(cartobj);
                        _Context.SaveChanges();
                    }
                    else{
                       
                        _Context.CartProjObject.Remove(proId);
                        cartobj.price -= product.Price;
                        var cartPro = cartobj.Product.Where(s => s.Id == product.Id).First();                       
                        cartobj.Product.Remove(cartPro);
                        _Context.CartObject.Update(cartobj);
                        _Context.SaveChanges();

                    }
                    
                }
                        return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

    }
}
