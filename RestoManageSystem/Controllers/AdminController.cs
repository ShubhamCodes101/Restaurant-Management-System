using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestoManageSystem.Models;
using System.IO;

namespace RestoManageSystem.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Home()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "Home"));
            }
        }

        public ActionResult AddRest()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "AddRest"));
            }
        }

        [HttpPost]
        public ActionResult AddRestaurant2(Restaurant r1, HttpPostedFileBase pfile1)
        {
            try
            {
                RMSEntities ctx = new RMSEntities();
                BinaryReader brd = new BinaryReader(pfile1.InputStream);
                byte[] bt = brd.ReadBytes((int)pfile1.InputStream.Length);
                r1.Rimages = bt;
                ctx.Restaurants.Add(r1);
                ctx.SaveChanges();
                TempData["rest_msg"] = "Restaurant Added";
                return RedirectToAction("AddRest");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public JsonResult Rest5(Restaurant b1)
        {
            RMSEntities stx = new RMSEntities();
            var d = from a in stx.Restaurants.ToList()
                    where a.Rname == b1.Rname
                    select a.Category;
            d = d.Distinct().ToList();
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult Branch3(Branch f1)
        {
            RMSEntities ctx = new RMSEntities();
            List<Mybranch> listM = new List<Mybranch>();
            foreach (var item in ctx.Branches.ToList())
            {
                if (f1.Rname == item.Rname && f1.bcity == item.bcity && f1.Location==item.Location)
                {
                    Mybranch m = new Mybranch();
                    m.Rname = item.Rname;
                    m.bcity = item.bcity;
                    m.Location = item.Location;
                    m.bimage = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.bimage));
                    m.bphonenum = item.bphonenum;
                    listM.Add(m);
                }
            }
            var data1 = from a in ctx.Branches.ToList() where a.Rname == f1.Rname && a.bcity == f1.bcity && a.Location == f1.Location select new { a.bphonenum, a.bimage };

            return Json(listM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FinalUpdateBranch(Branch r1, HttpPostedFileBase pfile3)
        {
            RMSEntities ctx = new RMSEntities();
            string dum = Request.Form["Location1"].ToString();
            if (pfile3 != null)
            {               
                BinaryReader brd = new BinaryReader(pfile3.InputStream);
                byte[] bt = brd.ReadBytes((int)pfile3.InputStream.Length);
                foreach (var item in ctx.Branches)
                {
                    if (r1.Rname == item.Rname && r1.bcity == item.bcity && r1.Location==item.Location)
                    {
                        item.bimage = bt;
                        item.bphonenum = (Int64)r1.bphonenum;
                        item.Location = dum;
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in ctx.Branches)
                {
                    if (r1.Rname == item.Rname && r1.bcity == item.bcity&& r1.Location==item.Location)
                    {
                        item.bphonenum = (Int64)r1.bphonenum;
                        item.Location = dum;
                        break;
                    }
                }
            }
            TempData["succ_msg"] = "Branch Updated Successfully !!!";
            ctx.SaveChanges();
            return RedirectToAction("EditRest");
        }

        public ActionResult AddBranch()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "Admin", "AddRest"));
            }
        }

        [HttpPost]
        public ActionResult AddBranch2(Branch r1, HttpPostedFileBase pfile1)
        {
            try
            {
                RMSEntities ctx = new RMSEntities();
                Branch obj = new Branch();
                var data = ctx.Branches.FirstOrDefault(i => (i.Location==r1.Location && i.bcity==r1.bcity && i.Rname==r1.Rname));
                
                if (data==null)
                {
                    BinaryReader brd = new BinaryReader(pfile1.InputStream);
                    byte[] bt = brd.ReadBytes((int)pfile1.InputStream.Length);
                    r1.bimage = bt;
                    ctx.Branches.Add(r1);

                    ctx.SaveChanges();
                    //ViewBag.Message = "Restaurant Added";
                    TempData["success"] = "Branch Added";
                    return RedirectToAction("AddBranch");
                }

                
                TempData["Error"] = "Duplicate Entry!";
                return RedirectToAction("AddBranch");
                
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        
        public JsonResult GetAllName()
        {
            RMSEntities stx = new RMSEntities();
            var d = from a in stx.Restaurants.ToList()
                    select a.Rname;
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddMenu()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "Admin", "AddRest"));
            }
        }

        [HttpPost]
        public ActionResult AddMenu2(Menu b1, HttpPostedFileBase pfile1)
        {
            RMSEntities ctx = new RMSEntities();
            string cat = Request.Form["type2"].ToString();
            try
            {
                foreach (var item in ctx.Restaurants)
                {
                    Menu obj = new Menu();
                    if (cat == "BOTH")
                    {
                        BinaryReader brd = new BinaryReader(pfile1.InputStream);
                        byte[] bt = brd.ReadBytes((int)pfile1.InputStream.Length);
                        b1.Itemimage = bt;
                        ctx.Menus.Add(b1);
                        TempData["menu_succ"] = "Menu Added";
                        break;
                    }
                    else
                    {
                        if (item.Rname == b1.Rname && item.Category == b1.Foodtype )
                        {
                            BinaryReader brd = new BinaryReader(pfile1.InputStream);
                            byte[] bt = brd.ReadBytes((int)pfile1.InputStream.Length);
                            b1.Itemimage = bt;
                            ctx.Menus.Add(b1);
                            TempData["menu_succ"] = "Menu Added";
                            break;
                        }
                        else
                        {
                            //TempData["dup_msg"] = "Kindly Check Data...";
                           // return RedirectToAction("AddMenu");
                        }
                    }
                }
                ctx.SaveChanges();
                
                return RedirectToAction("AddMenu");
            }
            catch (Exception ex)
            {
                return View("Error",
                    new HandleErrorInfo(ex, "Admin", "AddBranch"));
                //return View("Error");
            }
        }


        public JsonResult GetAllMenu(Menu m1)
        {
            RMSEntities stx = new RMSEntities();
            var d = from a in stx.Menus.ToList()
                    where a.Rname == m1.Rname
                    select a.Rname;
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Menu3(Menu f1)
        {
            RMSEntities ctx =new RMSEntities();
            List<Mymenu> listMenu = new List<Mymenu>();
            foreach (var item in ctx.Menus.ToList())
            {
                if (f1.Rname == item.Rname)
                {
                    Mymenu m = new Mymenu();
                    m.Itemsname = item.Itemsname;
                    m.price = (int)item.price;
                    m.Itemimage = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.Itemimage));
                    m.Foodtype = item.Foodtype;
                    listMenu.Add(m);
                }
            }
            var data1 = from a in ctx.Menus.ToList() where (a.Rname == f1.Rname) select a.Itemsname ;
            return Json(listMenu,
                  JsonRequestBehavior.AllowGet);
        }

        public JsonResult Menu5(Menu f1)
        {
            RMSEntities ctx = new RMSEntities();
            List<Mymenu> listMenu = new List<Mymenu>();
            foreach (var item in ctx.Menus.ToList())
            {
                if (f1.Rname == item.Rname)
                {
                    Mymenu m = new Mymenu();
                    m.Itemsname = item.Itemsname;
                    m.price = (int)item.price;
                    m.Itemimage = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.Itemimage));
                    m.Foodtype = item.Foodtype;
                    listMenu.Add(m);
                }
            }
            var data1 = from a in ctx.Menus.ToList() where (a.Rname == f1.Rname) select a.Itemsname;
            return Json(data1.ToList(),
                  JsonRequestBehavior.AllowGet);
        }

        public JsonResult Menu4(Menu f1)
        {
            RMSEntities ctx = new RMSEntities();
            List<Mymenu> listMenu = new List<Mymenu>();
            foreach (var item in ctx.Menus.ToList())
            {
                if (f1.Rname == item.Rname&&f1.Itemsname==item.Itemsname)
                {
                    Mymenu m = new Mymenu();
                    m.Itemsname = item.Itemsname;
                    m.price = (int)item.price;
                    m.Itemimage = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.Itemimage));
                    m.Foodtype = item.Foodtype;
                    listMenu.Add(m);
                }
            }
            var data1 = from a in ctx.Menus.ToList() where a.Rname == f1.Rname && a.Itemsname==f1.Itemsname select new { a.Itemsname,a.price};
            // Menu s = data1.ToList().FirstOrDefault(i => i.Itemsname == f1.Itemsname);
          
            return Json(listMenu,
                  JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FinalUpdateMenu(Menu r1, HttpPostedFileBase pfile1)
        {
            RMSEntities ctx = new RMSEntities();
      
            if (pfile1 != null)
            {
                BinaryReader brd = new BinaryReader(pfile1.InputStream);
                byte[] bt = brd.ReadBytes((int)pfile1.InputStream.Length);
                foreach (var item in ctx.Menus)
                {
                    if (r1.Rname == item.Rname &&r1.Itemsname == item.Itemsname)
                    {
                        item.Itemimage = bt;
                        item.price = (int)r1.price;
                        break;
                    }
                }
            }
            else
            {
               foreach (var item in ctx.Menus)
                {
                   if (r1.Rname == item.Rname && r1.Itemsname == item.Itemsname)
                   {
                      item.price = (int)r1.price;
                       break;
                  }
              }
           }
            ctx.SaveChanges();
            return RedirectToAction("EditMenu");
        }
      
        public JsonResult DelItem2(Menu f1)
        {
            RMSEntities ctx = new RMSEntities();
            foreach (var item in ctx.Menus)
            {
                if (f1.Rname == item.Rname && f1.Itemsname == item.Itemsname)
                {
                    ctx.Menus.Remove(item);
                    break;
                }
            }
            ctx.SaveChanges();
            return Json(f1);
        }

        public ActionResult EditRest()
        {
            return View();
        }

        public JsonResult GetAllName1()
        {
            RMSEntities stx = new RMSEntities();
            var d = from a in stx.Restaurants.ToList()
                    select a.Rname;
            d = d.Distinct().ToList();
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCity(Branch b1)
        {
            RMSEntities stx = new RMSEntities();
            var d = from a in stx.Branches.ToList() where a.Rname ==b1.Rname
                    select a.bcity;
            d = d.Distinct().ToList();
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLoc(Branch b1)
        {
            RMSEntities stx = new RMSEntities();
            var d = from a in stx.Branches.ToList()
                    where a.Rname == b1.Rname && a.bcity==b1.bcity
                    select a.Location;
            d = d.Distinct().ToList();
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelBranch2(Branch r1)
        {
            RMSEntities ctx = new RMSEntities();
            foreach (var item in ctx.Menus)
            {
                if (r1.Rname == item.Rname )
                {
                    ctx.Menus.Remove(item);
                }
            }
            ctx.SaveChanges();
            foreach (var item2 in ctx.Branches)
            {
                if (r1.Rname == item2.Rname && r1.bcity == item2.bcity && r1.Location == item2.Location)
                {
                    ctx.Branches.Remove(item2);
                    break;
                }
            }
            ctx.SaveChanges();
            return Json(r1);
        }
    }
}