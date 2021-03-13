using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestoManageSystem.Models;

namespace RestoManageSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult AdminLogin(string role)
        {
            return View();
        }

        public ActionResult UserLogin(string Role)
        {
            RMSEntities rtx = new RMSEntities();
            var list = from a in rtx.Branches select new { bcity=a.bcity};
            list = list.Distinct();
            
            ViewBag.list = new SelectList(list.ToList(), "","bcity");
            return View();
        }

        public ActionResult Logout()
        {
            Session["Role"] = null;
            Session["Username"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserSignup(SignupModel obj)
        {
            if(ModelState.IsValid)
            {
                RMSEntities rtx = new RMSEntities();
                var data = rtx.Customers.FirstOrDefault(i => i.Userid == obj.Userid);
                if(data != null)
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View("Signup");
                }
                Customer c = new Customer();
                c.Username = obj.Username;
                c.Userid = obj.Userid;
                c.password = obj.Password;
                c.Usercity = obj.Usercity;
                c.Uphonenum = long.Parse(obj.Userphno);
                rtx.Customers.Add(c);
                rtx.SaveChanges();

                return RedirectToAction("UserLogin");
            }
            else
            {
                return View("Signup");
            }
        }

        [HttpPost]
        public ActionResult ValidateUser(Credentials obj)
        {
            if(ModelState.IsValid)
            {
                RMSEntities mtx = new RMSEntities();
                Customer c1 = mtx.Customers.FirstOrDefault(i => i.Userid == obj.Userid);
                if(c1==null)
                {
                    TempData["userloginerror"] = "User not found!";
                    return RedirectToAction("UserLogin");
                }
                if (obj.Userid == c1.Userid && obj.Password == c1.password)
                {
                    Session["Userid"] = c1.Userid;
                    Session["Username"] = c1.Username;
                    Session["Usercity"] = obj.Usercity;
                    return RedirectToAction("Home", "User");                   /// add user function link here
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User name or password");
                    TempData["userloginerror"] = "Invalid User name or password";
                    return RedirectToAction("UserLogin");
                }
            }
            else
            {
                TempData["userloginerror"] = "Enter valid data";

                return RedirectToAction("UserLogin");
            }
        }

        [HttpPost]
        public ActionResult ValidateAdmin(admincred obj)
        {
            if(ModelState.IsValid)
            {
                if(obj.Userid=="admin" && obj.Password=="admin")
                {
                    Session["Username"] = "Admin";
                    return RedirectToAction("Home", "Admin");             // add admin functions link here
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User name or password");
                    TempData["adminloginerror"] = "Invalid User name or password";
                    return RedirectToAction("AdminLogin", "Account");
                }
            }
            else
            {
                TempData["adminloginerror"] = "Enter valid data";
                return RedirectToAction("AdminLogin","Account");
            }
        }
    }
}