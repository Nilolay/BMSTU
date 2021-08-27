using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebMarket.Models.Data;
using WebMarket.Models.ViewModels.Shop;

namespace WebMarket.Areas.Admin.Controllers
{

    
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                categoryVMList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVM(x))
                    .ToList();
            }
            return View(categoryVMList);
        }


        //POST Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // Declare id
            string id;

            using (Db db = new Db())
            {


                //check the category name is unique
                if (db.Categories.Any(x => x.Name == catName))
                {
                    return "titletaken";
                }


                //init the dto
                CategoryDTO dto = new CategoryDTO();
                //add to dto
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                //save

                db.Categories.Add(dto);
                db.SaveChanges();

                //get the id
                id = dto.id.ToString();
            }




            //return id
            return id;
            //return View();
        }

        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //declare page dto

                CategoryDTO dto;
                //set sorting for each page
                foreach (var catid in id)
                {
                    dto = db.Categories.Find(catid);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
        }


        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                CategoryDTO dto = db.Categories.Find(id);
                //remove the page
                db.Categories.Remove(dto);
                //save
                db.SaveChanges();
            }


            //redirect
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db())
            {

                //check category name is unique
                if (db.Categories.Any(x => x.Name == newCatName && x.id != id))
                {

                    return "titletaken";
                }
                else
                {
                    //get dto
                    CategoryDTO dto = db.Categories.Find(id);
                    // edit dto
                    dto.Name = newCatName;
                    dto.Slug = newCatName.Replace(" ", "-").ToLower();
                    //save 
                    db.SaveChanges();
                }

            }


            return " ";
            //return View();
        }



        //Get Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct(string ProductName)
        {
            // declare id
            ProductVM model = new ProductVM();
            //init dto
            using (Db db = new Db())
            {


                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                //check name is unique
                if (db.Products.Any(x => x.Name == ProductName))
                {
                    return Content("The product with the same name already exists!");
                }
                else
                {
                    //add dto
                    //db.Products.Add();
                    //save
                    db.SaveChanges();
                }



            }


            //return id
            return View(model);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }


            //name is unique?
            using (Db db = new Db())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }


            //declare product id
            int id;
            //init product dto

            using (Db db = new Db())
            {
                ProductDTO product = new ProductDTO();

                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower();
                product.Price = model.Price;
                product.Description = model.Description;
                product.CategoryId = model.CategoryId;

                CategoryDTO catDTO = db.Categories.FirstOrDefault(x => x.id == model.CategoryId);
                product.CategoryName = catDTO.Name;

                db.Products.Add(product);
                db.SaveChanges();

                //get the inserted id

                id = product.Id;

            }


            //set tempdata msg

            TempData["SM"] = "You have added a product!";


            #region Upload Image

            //Create necessary directories

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));



            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
            {
                Directory.CreateDirectory(pathString1);
            }
            if (!Directory.Exists(pathString2))
            {
                Directory.CreateDirectory(pathString2);
            }
            if (!Directory.Exists(pathString3))
            {
                Directory.CreateDirectory(pathString3);
            }
            if (!Directory.Exists(pathString4))
            {
                Directory.CreateDirectory(pathString4);
            }
            if (!Directory.Exists(pathString5))
            {
                Directory.CreateDirectory(pathString5);
            }
            //check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                //get file extension
                string ext = file.ContentType.ToLower();
                //verify extension

                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {

                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded! - wrong image extension");
                        return View(model);

                    }

                }


                //init image name
                string imageName = file.FileName;
                //save image name to dto

                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;

                    db.SaveChanges();
                }

                //set original and thumb image paths

                var path = string.Format("{0}//{1}", pathString2, imageName);
                var path2 = string.Format("{0}//{1}", pathString3, imageName);

                //save original
                file.SaveAs(path);

                //create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);

            }





            #endregion

            //redirect
            return RedirectToAction("AddProduct");
        }



        public ActionResult Products(int? page, int? catId)
        {
            //declare a list of product VMs
            List<ProductVM> listOfProductVM;

            //set page number
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
                // init the list
                listOfProductVM = db.Products.ToArray()
                                             .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                                             .Select(x => new ProductVM(x))
                                             .ToList();
                //populate categories select list
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                //set selected category
                ViewBag.SelectedCat = catId.ToString();

            }

            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 5);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            //return view with list
            return View(listOfProductVM);
        }

        // GET:  Admin/Shop/EditProduct/id
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //Declare product vm

            ProductVM model;

            using (Db db = new Db())
            {
                //Get the product
                ProductDTO dto = db.Products.Find(id);
                //Make sure product exists

                if (dto == null)
                {
                    return Content("That Product does not exist.");
                }


                //init model

                model = new ProductVM(dto);
                //make a select list

                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                //get all gallary images
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs")).Select(fn => Path.GetFileName(fn));
            }



            //return view with model
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            //get product id 
            int id = model.Id;
            //populate categories select list and gallery images
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs")).Select(fn => Path.GetFileName(fn));

            //check model state

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //make sure product name is unique

            using (Db db = new Db())
            {
                if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "That product Name is tsken!");
                    return View(model);

                }
            }

            //update product
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);

                dto.Name = model.Name;
                dto.Price = model.Price;
                dto.Slug = model.Name.Replace(" ", "-").ToLower();
                dto.CategoryId = model.CategoryId;
                dto.Description = model.Description;
                dto.ImageName = model.ImageName;


                CategoryDTO catDTO = db.Categories.FirstOrDefault(x => x.id == model.CategoryId);
                dto.CategoryName = catDTO.Name;


                db.SaveChanges();
            }
            //set tempdata msg
            TempData["SM"] = "Product have been updated!";


            #region Image Upload

            //cheack for file upload
            if (file != null && file.ContentLength > 0)
            {
                // get the extension
                string ext = file.ContentType.ToLower();
                //verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        ModelState.AddModelError("", "The image was not uploaded! - wrong image extension");
                        return View(model);

                    }

                }

                //set upload directory paths
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));




                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
                //delete files from directories


                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                {
                    file2.Delete();
                }

                foreach (FileInfo file3 in di2.GetFiles())
                {
                    file3.Delete();
                }
                //save image name

                string ImageName = file.FileName;

                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = ImageName;

                    db.SaveChanges();

                }
                //save original and thumb images
                var path = string.Format("{0}//{1}", pathString1, ImageName);
                var path2 = string.Format("{0}//{1}", pathString2, ImageName);

                //save original
                file.SaveAs(path);

                //create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }


            #endregion

            return RedirectToAction("EditProduct");
        }


        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            //delete product from db
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);
                db.Products.Remove(dto);
                db.SaveChanges();
            }
            //delete product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathstring = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathstring))
            {
                Directory.Delete(pathstring, true);
            }
            //redirect
            return RedirectToAction("Products");
        }

        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            //loop throught files 
            foreach (string fileName in Request.Files)
            {
                //init the files 
                HttpPostedFileBase file = Request.Files[fileName];
                //check it's not null 

                if (file != null && file.ContentLength > 0)
                {
                    //set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));


                    string pathstring1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathstring2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");
                    //SET image paths

                    var path = string.Format("{0}\\{1}", pathstring1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathstring2, file.FileName);
                    //save original and thumb
                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }


            }

        }

        [HttpPost]
        //Post Admin/Shop/DeleteImage
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
            {
                System.IO.File.Delete(fullPath1);
            }
            if (System.IO.File.Exists(fullPath2))
            {
                System.IO.File.Delete(fullPath2);
            }
        }
    }
}