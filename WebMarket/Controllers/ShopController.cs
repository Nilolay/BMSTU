using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMarket.Models.Data;
using WebMarket.Models.ViewModels.Shop;

namespace WebMarket.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CategoryMenuPartial()
        {
            //Declare the list of CategoryVM
            List<CategoryVM> categoryVMList;
            //Init the list
            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();

                
            }
            //return partial view
            return PartialView(categoryVMList);
        }

        //get /shop/category/name
        public ActionResult Category(string name)
        {
            //declare category VM list
            List<ProductVM> productVmList;
           
            using (Db db =new Db())
            {

                //get category id
                CategoryDTO Categorydto = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = Categorydto.id;
                //init the list 

                productVmList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductVM(x)).ToList();
                //get category name
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;

                //CategoryDTO dto = db.Categories.Find()
            }
            

            //return view with list
            return View(productVmList);
        }

        //get /shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            //declare vm and dto
            ProductVM model;

            using (Db db = new Db())
            {

              //init product id
              int id = 0;
                //check if product exists
                if (!db.Products.Any(x=> x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }
                //init product dto
                ProductDTO dto = db.Products.Where(x => x.Slug.Equals(name)).FirstOrDefault();
                //get inserted id 
                id = dto.Id;
                //init model
                model = new ProductVM(dto);
                //get gallery images
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs")).Select(fn => Path.GetFileName(fn));
            }
            //return view with the model
            return View("ProductDetails", model);
        }
    }
}