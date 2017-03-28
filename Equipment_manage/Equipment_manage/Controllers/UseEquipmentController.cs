using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Equipment_manage.Models;
using Equipment_manage.Models.Authen;
using PagedList;

namespace Equipment_manage.Controllers
{
    public class UseEquipmentController : Controller
    {
        private Equipment DB = new Equipment();
        // GET: /UseEquipment/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetPersonEquipment(int pageSize, int pageNumber, string keyword = "", int? id = null, string sortOrder = "")
        {
            var user = (System.Web.HttpContext.Current.User as MyUserDataPrincipal).userdata.e_Name;
            var start = DB.DecivesInfo.Where(u => u.di_User == user).Select(u => new { di_EqId = u.di_EqId, di_Name = u.di_Name, di_Position = u.di_Position, di_Status = u.di_Status }).OrderBy(u => u.di_Name);
            var reust = start.ToPagedList(pageNumber, pageSize);
            var total = start.Count();
            var row = reust;
            return Json(new { total = total, rows = row }, JsonRequestBehavior.AllowGet);
        }
	}
}