using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMarket.Models.Data;
using WebMarket.Models.ViewModels.Pages;

namespace WebMarket.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare Page of VM
            List<PageVM> pageslist;
            //Init the list
            using (Db db = new Db())
            {
                pageslist = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Return view with list
            return View(pageslist);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {


                //declare slug
                string slug;

                //init page dto
                PageDTO dto = new PageDTO();
                //dto title
                dto.Title = model.Title;
                //check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //make sure title and slug are unique

                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("","The title or slug is already exists");
                    return View(model);
                }

                //dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //save dto
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //set tempdata msg
            TempData["SM"] = "You have added a new page";

            //redirect
            return RedirectToAction("AddPage");
        }

        public ActionResult EditPage(int id)
        {
            //Declare Page VM
            PageVM model;

            using (Db db = new Db())
            {
                //Get Page
                PageDTO dto = db.Pages.Find(id);
                //confirm page exist
                if (dto == null)
                {
                    return Content("This page does not exist");
                }
                //Init PageVM

                model = new PageVM(dto);

            }

            return View(model);

        }

        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //GetHashCode Page ID
                int id = model.Id;
                //Declare Slug
                string slug = "home";

                //Get the Page
                PageDTO dto = db.Pages.Find(id);


                //DTO title
                dto.Title = model.Title;
                //check for the slug and set if it is needed
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }


                //make sure title and slug are unique

                if (db.Pages.Where(x => x.Id != model.Id).Any(x => x.Title == model.Title) || db.Pages.Where(x => x.Id != model.Id).Any(x => x.Slug == slug) )
                {
                    ModelState.AddModelError("", "This title or slug already exists");
                    return View(model);
                }

                //dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //save dto
                db.SaveChanges();
            }

            //set  tempdata msg
            TempData["SM"] = "You have edited the page!";

              //redirect
            return RedirectToAction("EditPage");
        }


        public ActionResult PageDetails(int id)
        {
            //Declare the Pagevm
            PageVM model;

            using (Db db = new Db())
            {


                //Get the page
                PageDTO dto = db.Pages.Find(id);
                //confirm the page exists

                if (dto == null)
                {
                    return Content("The page does not exist!");
                }
                //init the model
                model = new PageVM(dto);

            }


            return View(model);
        }

        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);
                //remove the page
                db.Pages.Remove(dto);
                //save
                db.SaveChanges();
            }


            //redirect
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //declare page dto

                PageDTO dto;
                //set sorting for each page
                foreach (var pageid in id)
                {
                    dto = db.Pages.Find(pageid);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
        }

        [HttpGet]
        public ActionResult EditSidebar()
        {
            //declare the model
            SidebarVM model;

            using (Db db = new Db())
            {
                //get the dto
                SidebarDTO dto = db.Sidebar.Find(1);
                //init the model
                model = new SidebarVM(dto);
            }


            //return view with model
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                //get the dto
                SidebarDTO dto = db.Sidebar.Find(1);
                //dto the body
                dto.body = model.Body;
                //save
                db.SaveChanges();
            }


            //set tempdata
                TempData["SM"] = "You have edited sidebar";

                //redirect
                return RedirectToAction("EditSidebar");
        }
        }
}