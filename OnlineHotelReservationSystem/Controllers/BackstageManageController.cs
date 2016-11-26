using OnlineHotelReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineHotelReservationSystem.Controllers
{
    public class BackstageManageController : Controller
    {
        // GET: BackstageManage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginSysAdmin()
        {
            return View();
        }
        public ActionResult LoginSysAdminSave(SysAdmin model)
        {
            var state = false;
            if (ModelState.IsValid)
            {
                var db = new HotelDatabase();

                var lst1 = db.SysAdmins.AsQueryable();
                lst1 = lst1.Where(o => o.SysAdminName.Contains(model.SysAdminName));
                var lst2 = lst1.ToList();
                foreach (var q in lst2)
                {
                    if (q.SysAdminName == model.SysAdminName && q.SysAdminPassword == model.SysAdminPassword)
                    {
                        state = true;
                    }
                    else state = false;
                }
            }
            if ( state == true)
            {
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Account");
            //return RedirectToAction("Index","Home");
        }

        public ActionResult RegisterSysAdmin()
        {

            return View();
        }
        public ActionResult RegisterSysAdminSave(SysAdmin model)
        {
            if (ModelState.IsValid)
            {
                var db = new HotelDatabase();
                db.Database.CreateIfNotExists();

                var sysAdmin = new SysAdmin();
                sysAdmin.SysAdminName = model.SysAdminName;
                sysAdmin.SysAdminPassword = model.SysAdminPassword;
                //sysAdmin.SysAdminIdCode = model.SysAdminIdCode;

                db.SysAdmins.Add(sysAdmin);
                db.SaveChanges();
            }
            return View("LoginSysAdmin");
        }
    }
}