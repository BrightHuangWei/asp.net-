using Microsoft.AspNet.Identity;
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

        public ActionResult LoginSysAdminState()
        {
            return View();
        }

        /// <summary>
        /// 单个用户，查看自己的所有订单
        /// </summary>
        /// <returns></returns>
        public ActionResult SingleOrderList()
        {
            var db = new HotelDatabase();
            var nowUser = User.Identity.GetUserName();
            var singleList = db.ReserveInfos.AsQueryable().Where(o => o.UserName.Equals(nowUser));
            ViewBag.SingleUserOrderList = singleList.OrderBy(o => o.OrderId).ToList();
            return View();
        }
        /// <summary>
        /// 用户退订，删除订单
        /// </summary>
        /// <returns></returns>
        public ActionResult SingleOrderListDelete(int OrderId)
        {
            var db = new HotelDatabase();
            var singleDel = db.ReserveInfos.First(o => o.OrderId == OrderId);
            db.ReserveInfos.Remove(singleDel);
            db.SaveChanges();
            return RedirectToAction("SingleOrderList");
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="model">接收数据</param>
        /// <returns></returns>
        public ActionResult LoginSysAdminSave(SysAdmin model)
        {
            var state = false;
            var db = new HotelDatabase();
            if (ModelState.IsValid)
            {
                db.Database.CreateIfNotExists();

                var lst = db.SysAdmins.AsQueryable();
                lst = lst.Where(o => o.SysAdminName.Contains(model.SysAdminName));
                foreach (var q in lst)
                {
                    if (q.SysAdminName == model.SysAdminName && q.SysAdminPassword == model.SysAdminPassword)
                    {
                        state = true;
                    }
                    else state = false;
                }
            }
            if (state == true)
            {
                try
                {
                    var stateLogin = new SysAdminLogin();
                    stateLogin.SysAdminLoginState = "true";
                    stateLogin.SysAdminName = model.SysAdminName;
                    ViewBag.SysAdminName = model.SysAdminName;
                    db.SysAdminLogins.Add(stateLogin);
                    db.SaveChanges();

                    return RedirectToAction("OrderListIndex", "BackstageManage");
                }
                catch (ArgumentNullException e)
                {
                    return Content("<script> alert('该账号已登录！'); </script>");
                }

            }
            else return View("LoginSysAdmin");
        }
        /// <summary>
        /// 管理员退出登录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult LoginSysAdminExit(string name)
        {
            var db = new HotelDatabase();
            var state = db.SysAdminLogins.First(o => o.SysAdminName == name);
            db.SysAdminLogins.Remove(state);
            db.SaveChanges();
            return RedirectToAction("LoginSysAdmin", "BackstageManage");
        }
        public ActionResult RegisterSysAdmin()
        {
            return View();
        }

        /// <summary>
        /// 注册信息保存
        /// </summary>
        /// <param name="model">接收数据</param>
        /// <returns></returns>
        public ActionResult RegisterSysAdminSave(SysAdmin model)
        {
            if (ModelState.IsValid)
            {
                var db = new HotelDatabase();
                db.Database.CreateIfNotExists();

                var sysAdmin = new SysAdmin();
                sysAdmin.SysAdminName = model.SysAdminName;
                sysAdmin.SysAdminPassword = model.SysAdminPassword;
                sysAdmin.SysAdminIdCode = model.SysAdminIdCode;
                db.SysAdmins.Add(sysAdmin);
                db.SaveChanges();
            }
            return View("LoginSysAdmin");
        }
        /// <summary>
        /// 管理员删除订单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public ActionResult OrderListDelete(int OrderId)
        {
            var db = new HotelDatabase();
            var singleDel = db.ReserveInfos.First(o => o.OrderId == OrderId);
            db.ReserveInfos.Remove(singleDel);
            db.SaveChanges();
            return RedirectToAction("OrderListIndex");
        }

        /// <summary>
        /// 保存预定信息
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSave(ReserveInfo model)
        {
            var flag = false;
            if (HttpContext.User.Identity.IsAuthenticated == true) //已登录
            {
                var db = new HotelDatabase();
                db.Database.CreateIfNotExists();
                var saveInfo = new ReserveInfo();

                var lst = db.Suites.AsQueryable().Where(o => o.SuiteType.Contains(model.SuiteType)); ///查询价格
                var price = 100;
                foreach (var q in lst)
                {
                    if (true)
                    {
                        price = q.SuitePrice;
                        break;
                    }
                }
                //var d1 = model.ArrivalTime;
                //var d2 = model.ArrivalTime;
                //var dtArrival = d1.ToString("yyyy/MM/dd");
                //var dtLevel = d2.ToString("yyyy/MM/dd");
                saveInfo.UserName = User.Identity.GetUserName(); //当前登录用户
                saveInfo.OrderName = model.OrderName;
                saveInfo.OrderIdCard = model.OrderIdCard;
                saveInfo.OrderTime = DateTime.Now;
                saveInfo.SuiteType = model.SuiteType;
                saveInfo.OrderNunber = model.OrderNunber;
                saveInfo.ArrivalTime = model.ArrivalTime;
                saveInfo.LeaveTime = model.LeaveTime;
                saveInfo.Tel = model.Tel;
                saveInfo.TotalPrice = price * model.OrderNunber;

                db.ReserveInfos.Add(saveInfo);
                db.SaveChanges();
                flag = true;
            }
            if (flag == true)
            {
                //return Content("<script> alert('预订成功'); </script>");
                return View();
            }
            else
                return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 订单管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderListIndex()
        {
            var db = new HotelDatabase();

            var reserveInfoData = new ReserveInfo();
            var lst = db.ReserveInfos.AsQueryable();

            ViewBag.ReserveInfoData = lst.OrderByDescending(o => o.OrderId).ToList();
            ViewData["sysname"] = GetSysAdminName();
            return View();
        }


        /// <summary>
        /// 订单修改页面处理
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public ActionResult OrderListEdit(int OrderId)
        {
            var db = new HotelDatabase();
            var changeList = db.ReserveInfos.First(o => o.OrderId == OrderId);

            ViewData.Model = changeList;
            ViewData["sysname"] = GetSysAdminName();
            return View();
        }
        /// <summary>
        /// 订单修改保存
        /// </summary>
        /// <param name="changeSaveModel"></param>
        /// <returns></returns>
        public ActionResult OrderListEditSave(ReserveInfo changeSaveModel)
        {
            var db = new HotelDatabase();
            var changeSave = db.ReserveInfos.First(o => o.OrderId == changeSaveModel.OrderId);
            
            changeSave.OrderName = changeSaveModel.OrderName;
            changeSave.OrderIdCard = changeSaveModel.OrderIdCard;
            changeSave.Tel = changeSaveModel.Tel;
            
            db.SaveChanges();

            return RedirectToAction("OrderListIndex");
        }
        public ActionResult UserIndex()
        {
            var db = new HotelDatabase();
            var costomerData = new Customer();
            var lst = db.Customers.AsQueryable();
            ViewBag.CustomerData = lst.OrderByDescending(o => o.CustomerId).ToList();
            ViewData["sysname"] = GetSysAdminName();
            return View();
        }
        /// <summary>
        /// 房型管理
        /// </summary>
        /// <returns></returns>
        public ActionResult SuiteIndex()
        {
            var db = new HotelDatabase();

            var suiteData = new Suite();
            var lst = db.Suites.AsQueryable();

            ViewBag.SuiteData = lst.OrderByDescending(o => o.SuiteType).ToList();
            ViewData["sysname"] = GetSysAdminName();
            return View();
        }


        /// <summary>
        /// 增加房型 in SuiteIndex
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddSuite(Suite model)
        {
            var db = new HotelDatabase();
            var addSuite = new Suite();

            addSuite.SuiteType = model.SuiteType;
            addSuite.SuitePrice = model.SuitePrice;
            addSuite.TotalNumber = model.TotalNumber;
            addSuite.OddNumber = addSuite.TotalNumber; //剩余数量等于总数量

            db.Suites.Add(addSuite);
            db.SaveChanges();

            return RedirectToAction("SuiteIndex");
        }

        /// <summary>
        /// 删除房型
        /// </summary>
        /// <param name="SuiteId"></param>
        /// <returns></returns>
        public ActionResult SuiteDelete(string SuiteType)
        {
            var db = new HotelDatabase();
            var suiteDel = db.Suites.First(o => o.SuiteType == SuiteType);
            db.Suites.Remove(suiteDel);
            db.SaveChanges();
            return RedirectToAction("SuiteIndex");
        }
        /// <summary>
        /// 提升用户等级
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public ActionResult UserLevelUpgrade(int Id)
        {
            var db = new HotelDatabase();
            var changeLevel = db.Customers.First(o => o.CustomerId.Equals(Id));
            if (changeLevel.CustomerLevel == "普通用户")
            {
                changeLevel.CustomerLevel = "VIP1";
            }
            else if (changeLevel.CustomerLevel == "VIP1")
            {
                changeLevel.CustomerLevel = "VIP2";
            }
            else
                changeLevel.CustomerLevel = "VIP3";
            db.SaveChanges();
            return RedirectToAction("UserIndex");
        }
        /// <summary>
        /// 降低用户等级
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public ActionResult UserLevelDowngrade(int id)
        {
            var db = new HotelDatabase();
            var changeLevel = db.Customers.First(o => o.CustomerId.Equals(id));

            if (changeLevel.CustomerLevel == "VIP3")
            {
                changeLevel.CustomerLevel = "VIP2";
            }
            else if (changeLevel.CustomerLevel == "VIP2")
            {
                changeLevel.CustomerLevel = "VIP1";
            }
            else
                changeLevel.CustomerLevel = "普通用户";
            db.SaveChanges();
            return RedirectToAction("UserIndex");
        }

        public string GetSysAdminName()
        {
            var db = new HotelDatabase();
            var lst = db.SysAdminLogins.AsQueryable();
            var name = "q";
            foreach (var q in lst)
            {
                if (q.SysAdminName != null)
                {
                    name = q.SysAdminName;
                }
            }
            return name;
        }

    }


}