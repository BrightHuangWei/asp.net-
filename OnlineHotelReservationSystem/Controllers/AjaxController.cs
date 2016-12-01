using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineHotelReservationSystem.Controllers
{
    public class AjaxController : Controller
    {
        // GET: Ajax
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SuiteList(Suite model)
        {

            var db = new HotelDatabase();
            var addSuite = new Suite();
            addSuite.SuiteType = model.SuiteType;
            addSuite.SuitePrice = model.SuitePrice;
            addSuite.TotalNumber = model.TotalNumber;

            db.Suites.Add(addSuite);
            db.SaveChanges();

            var suiteData = new Suite();
            var lst = db.Suites.AsQueryable();

            var res = new JsonResult();

            var SuiteData = db.Suites.AsQueryable().OrderByDescending(o => o.SuiteType).ToList();

            res.Data = SuiteData;

            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。  
            return Json(GetResult(false, "添加成功", res));

        }

        public static object GetResult(bool rel, string msg, object data)
        {
            return new Dictionary<string, object> { { "rel", rel }, { "msg", msg }, { "obj", data } };

        }
       
    }
}