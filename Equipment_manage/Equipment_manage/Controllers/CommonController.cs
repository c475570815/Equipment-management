using Equipment_manage.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Equipment_manage.Controllers
{
    public class CommonController : Controller
    {
        // GET: /Common/
        [HttpGet]
        public JsonResult GetAllpeople()
        {
            var rows = EmployeesServices.GetAllPeople();
            var total = rows.Count();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllDepartment()
        {
            var rows = DepartmentServices.GetAllRecorde();
            var total = rows.Count();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAlltype()
        {
            var rows = DevicesTypesServices.GetAllRecord();
            var total = rows.Count();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
	}
}