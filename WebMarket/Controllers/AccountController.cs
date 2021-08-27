using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMarket.Models.Data;
using WebMarket.Models.ViewModels.Account;

namespace WebMarket.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("user-profile");
            }


            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //check user is valid
            bool isValid = false;

            using (Db db = new Db())
            {
                if (db.Users.Any(x=>x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
                {
                    isValid = true;
                }
            }

            if (!isValid)
            {
                ModelState.AddModelError("","Username or password is not valid!");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username,model.RememberME);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberME));
            }
            
        }


        // GET: account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }


        // POST: account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount",model);
            }
            //check if password match
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("","Password is match.");
                return View("CreateAccount", model);
            }


            //make sure user is unique
            using (Db db = new Db())
            {
                
                if (db.Users.Any(x => x.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " is taken! ");
                    model.Username = "";
                    return View("CreateAccount", model);
                }
            
            //create userDTO
            UserDTO dto = new UserDTO()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                Username = model.Username,
                Password = model.Password
            };

                //add the DTO
                db.Users.Add(dto);
                //Save
                db.SaveChanges();
                //Add to UserRolesDTO
                int id = dto.Id;
                UserRoleDTO roledto = new UserRoleDTO
                {
                    UserId = id,
                    RoleId = 2
                };

                db.UserRoles.Add(roledto);
                db.SaveChanges();
            }
            //create tempdata msg
            TempData["SM"] = "Account was created!";

            //redirect
            return Redirect("~/account/login");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }
        public ActionResult AccountPartialVM()
        {
            string username = User.Identity.Name;
            using (Db db = new Db())
            {
                
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                AccountVM model = new AccountVM()
                {
                    UserId = dto.Id,
                    Ammount = db.Accounts.FirstOrDefault(x => x.UserId == dto.Id).Ammount
                };
                return PartialView(model);
            }

            
        }

        public ActionResult Accountamt()
        {
            string username = User.Identity.Name;
            using (Db db = new Db())
            {

                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                AccountVM model = new AccountVM()
                {
                    UserId = dto.Id,
                    Ammount = db.Accounts.FirstOrDefault(x => x.UserId == dto.Id).Ammount
                };
                ViewBag.Username = dto.FirstName + " " + dto.LastName;
                return View(model);
            }

            
        }

            public ActionResult UserNavPartial()
        {

            //get username
            string username = User.Identity.Name;
            //declare model
            UserNavPartialVM model;


            using (Db db = new Db())
            {
                //get the user
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                //build the model

                model = new UserNavPartialVM()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

            }


            //return partial with the model
            return PartialView(model);
        }


        // get account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            //get username 
            string username = User.Identity.Name;

            //declare model
            UserProfileVM model;

            using (Db db = new Db())
            {
                //get user
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                //build model
                model = new UserProfileVM(dto);
            }


            //return view with model
            return View("UserProfile",model);
        }


        //post account//user-profile
        [HttpPost]
        [ActionName("user-profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Profile is not valid");
                return View("UserProfile", model);
            }
            //check if password match  if need be
            if (string.IsNullOrWhiteSpace(model.Password) && model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password is not matched!");
                return View("UserProfile", model);
            }
            
             

            using (Db db = new Db())
            {
                //get username
                string username = User.Identity.Name;
                //make sure username is unique
                if (db.Users.Where(x=>x.Id!=model.Id).Any(x => x.Username== username))
                {
                    ModelState.AddModelError("", "Username is already taken!");
                    model.Username = "";
                    return View("UserProfile", model);

                }
                //edit dto
                UserDTO dto = db.Users.Find(model.Id);

                dto.Username = model.Username;
                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }
                //save
                db.SaveChanges();
            }




            //set temp data msg
            TempData["SM"] = "You have edited your profile!";



            //redirect
            return Redirect("~/account/user-profile");
        }
    }
}