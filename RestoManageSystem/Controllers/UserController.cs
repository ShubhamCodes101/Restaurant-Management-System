using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestoManageSystem.Models;
using System.IO;
using RestoManageSystem;


namespace RestoManageSystem.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Order_placed()
        {
            return View();
        }

        public ActionResult Order()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "User", "Order"));
                //return View("Error");
            }
        }

        public JsonResult GetRests2()
        {
            RMSEntities ctx = new RMSEntities();
            var data = from a in ctx.Branches.ToList() where a.bcity == Session["Usercity"].ToString() 
                       select new { name = a.Rname };
            var data1 = from a in ctx.Restaurants.ToList() join b in data.ToList() on a.Rname equals b.name 
                        select new { type = a.Category };
            string s = data1.ToList()[0].type;

            List<UserRest> listRest = new List<UserRest>();
            foreach (var item in ctx.Branches.ToList())
            {
                if (Session["Usercity"].ToString() == item.bcity)
                {
                    UserRest u = new UserRest();
                    u.Rname = item.Rname;
                    u.Location = item.Location;
                    u.bcity = item.bcity;
                    u.Category = s;
                    u.bimage = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.bimage));
                    listRest.Add(u);
                }
            }
            return Json(listRest,
                   JsonRequestBehavior.AllowGet);
        }

        public JsonResult Sessions(Menu f1)
        {
            Session["Rname"] = f1.Rname;
            //Session["Branch"] = f1.Branch;
            return Json(f1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectMenu()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "User", "SelectMenu"));
                //return View("Error");
            }
        }

        public JsonResult GetMenu2()
        {
            RMSEntities ctx = new RMSEntities();
            List<Mymenu> listMenu = new List<Mymenu>();
            foreach (var item in ctx.Menus.ToList())
            {
                if (Session["Rname"].ToString() == item.Rname)
                {
                    Mymenu u = new Mymenu();
                    u.Itemsname = item.Itemsname;
                    u.price = (int)item.price;
                    u.Itemimage = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.Itemimage));
                    listMenu.Add(u);
                }
            }
            return Json(listMenu,
                   JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddToCart2(Cart c1)
        {
            RMSEntities ctx = new RMSEntities();

            var data = from a in ctx.Menus.ToList()
                       where a.Rname == Session["Rname"].ToString()
                                    && a.Itemsname == c1.Fooditems
                       select new
                       {
                           a.Itemsname,
                           a.price,
                           a.Rname,
                       };
            foreach (var item in data)
            {
                Cart c = new Cart();
                c.Fooditems = item.Itemsname;
                c.Price = item.price;
                c.Rname = item.Rname;
                c.Quantity = c1.Quantity;
                ctx.Carts.Add(c);
                ctx.SaveChanges();
                break;
            }
            return Json(c1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyCart()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "User", "MyCart"));
                //return View("Error");
            }
        }

        public JsonResult GetCart2()
        {
            RMSEntities ctx = new RMSEntities();
            //List<UserRest> listRest = new List<UserRest>();
            //foreach (var item in ctx.Restaurants.ToList())
            //{

            //        UserRest u = new UserRest();
            //        u.RName = item.RName;
            //        u.Branch = item.Branch;
            //        u.RType = item.RType;
            //        u.Logo = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.Logo));
            //        listRest.Add(u);

            //}
            var data = (from a in ctx.Carts.ToList()
                            //where (r1.City==a.City && r1.RName==a.RName)
                        select new
                        {
                            a.Fooditems,
                            a.Price,
                            a.Rname,
                            a.Quantity
                        }).ToList();

            return Json(data,
                   JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalPrice2()
        {
            RMSEntities ctx =
                                 new RMSEntities();
            int data = 0;
            foreach (var item in ctx.Carts.ToList())
            {
                data = data + ((int)item.Price * (int)item.Quantity);
            }
            return Json(data,
                  JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteItem2(Cart F)
        {
            RMSEntities ctx = new RMSEntities();
            foreach (var item in ctx.Carts.ToList())
            {
                if (item.Fooditems == F.Fooditems)
                {
                    ctx.Carts.Remove(item);
                    break;
                }
            }

            ctx.SaveChanges();
            return Json(F, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeQuantity2(Cart F)
        {
            RMSEntities ctx = new RMSEntities();
            foreach (var item in ctx.Carts.ToList())
            {
                if (item.Fooditems == F.Fooditems)
                {
                    item.Quantity = F.Quantity;
                    break;
                }
            }
            ctx.SaveChanges();
            return Json(F, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyHistory()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "User", "MyHistory"));
                //return View("Error");
            }
        }

        public JsonResult PlaceOrder2()
        {
            
            {
                RMSEntities ctx = new RMSEntities();
                foreach (var item in ctx.Carts)
                {
                    OrderedFood h1 = new OrderedFood()
                    {
                        Userid = Session["Userid"].ToString(),
                        Rname = item.Rname,
                        Itemname = item.Fooditems,
                        Price = item.Price,
                        Quantity = item.Quantity.ToString(),
                        Date = DateTime.Now
                    };
                    ctx.OrderedFoods.Add(h1);
                }
                var rows = from a in ctx.Carts
                           select a;
                foreach (var row in rows)
                {
                    ctx.Carts.Remove(row);
                }
                ctx.SaveChanges();
                int d = 1;
                return Json(d, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult GetHistory2()
        {
            RMSEntities ctx = new RMSEntities();
            var data = (from a in ctx.OrderedFoods.ToList()
                        where (a.Userid == Session["Userid"].ToString())
                        select new
                        {
                            a.Userid,
                            a.Rname,
                            a.Itemname,
                            a.Price,
                            a.Quantity,
                            Date = a.Date.ToString()
                            //a.DaTi
                            //DaTi = a.DaTi.ToString().Substring(
                            //        0, a.ReleaseDate.ToString().IndexOf(' ')),
                            //DaTi = a.DaTi.ToString()
                        }).ToList();
            ctx.SaveChanges();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}