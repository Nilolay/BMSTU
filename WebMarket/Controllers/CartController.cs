using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMarket.Models.Data;
using WebMarket.Models.ViewModels.Account;
using WebMarket.Models.ViewModels.Cart;

namespace WebMarket.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //init the cart list 
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty!";
                return View();
            }

            //calculate total and save to viewbag

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;

            //return view with model
            
            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //Init cart vm
            CartVM model = new CartVM();
            //init quantity
            int qty = 0;
            //init price

            decimal price = 0m;
            //check for cart session
            if (Session["cart"] != null)
            {
                //get total qty and price
                var list = (List<CartVM>)Session["cart"];
                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Price * item.Quantity;
                }
                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                //or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }


            

            //return partial and view with model
            return PartialView(model);
        }

        
        public ActionResult AddToCartPartial(int id)
        {
            //Init cart VM list
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //Init cartVM
            CartVM model = new CartVM();

            using (Db db = new Db())
            {
                //get the product
                ProductDTO product = db.Products.Find(id);

                //check if the product is alredy in cart 
                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);
                //if not add new
                if (productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.ImageName
                    });

                }
                else
                {
                    //if it is increment
                    productInCart.Quantity++;
                }

            }


            //get total qty and price in the model
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;
            //save cart back to session
            Session["cart"] = cart;
            //return partialview with the model
            return PartialView(model);
        }

        public JsonResult IncrementProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                model.Quantity++;

                var result = new { qty = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }



        // get cart/DecrementProduct
        public JsonResult DecrementProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                if (model.Quantity > 0)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity=0;
                    cart.Remove(model);
                }
               

                var result = new { qty = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        // get cart/RemoveProduct
        public void RemoveProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                
                
                cart.Remove(model);        
                


                

                
            }
        }

        public ActionResult checkout()
        {
            
            List<CartVM> cart = Session["cart"] as List<CartVM>;
            //  DateTime.Now
            using (Db db = new Db())
            {
                string username = User.Identity.Name;
                if (cart == null || cart[0] == null)
                {
                    return RedirectToAction("Index");
                }
                OrderDTO dto = new OrderDTO()
                {
                    UserId = db.Users.FirstOrDefault(x => x.Username == username).Id,
                    CreatedAt = DateTime.Now
                };
                decimal total = 0;
                foreach (var item in cart)
                {
                    total += item.Price * item.Quantity;
                    if (total > db.Accounts.Where(x=>x.UserId==dto.UserId).FirstOrDefault().Ammount)
                    {
                        ModelState.AddModelError("", "Money is not enought." );
                        TempData["Error"] = "Money is not enought.";
                        return RedirectToAction("Index");
                    }
                }


                 foreach (var item in cart)
                {
                    StorageDTO proddto = db.Storage.Find(item.ProductId);

                    if (item.Quantity > proddto.Stored)
                    {

                        ModelState.AddModelError("", "Storage is out of" + item.ProductName + ". You can order only " + proddto.Stored + "units of this product");
                        TempData["Error"] = "Storage is out of" + item.ProductName + ". You can order only " + proddto.Stored + "units of this product";
                        return RedirectToAction("Index");
                    }

                }
                AccountDTO asdto = db.Accounts.Where(x => x.UserId == dto.UserId).FirstOrDefault();
                asdto.Ammount -= total;
                db.Orders.Add(dto);
                foreach (var item in cart)
                {
                    
                    OrderDetailsDTO detdto = new OrderDetailsDTO()
                    {
                        OrderId = dto.OrderId,
                        ProductId = item.ProductId,
                        UserId = dto.UserId,
                        Quantity = item.Quantity
                    };
                    StorageDTO storedto = db.Storage.Find(item.ProductId);
                    storedto.Stored -= item.Quantity;
                    db.SaveChanges();
                    db.OrderDetails.Add(detdto);
                }
                
            }
            TempData["SM"] = "You make an order!";
            return RedirectToAction("Index");
        }

        public ActionResult Orders()
        {

            OrderDetailsVM model = new OrderDetailsVM();
            using (Db db = new Db())
            {
               // OrderDetailsDTO dto

            }
            return View();
        }

        public ActionResult OrderDetails()
        {

            OrderDetailsVM model = new OrderDetailsVM();
            using (Db db = new Db())
            {
               // OrderDetailsDTO dto 

            }
            return View();
        }
    }
}