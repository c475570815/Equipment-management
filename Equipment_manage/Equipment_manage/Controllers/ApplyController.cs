using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Equipment_manage.Models;
using Equipment_manage.Models.Authen;
using Equipment_manage.DAL;

namespace Equipment_manage.Controllers
{
    public class ApplyController : Controller
    {
        private Equipment DB = new Equipment();
        //
        // GET: /Apply/
        [MyAuthorize(Roles="admin,user")]
        public ActionResult RepairApply()
        {
            return View();
        }
        [HttpPost]
        [MyAuthorize(Roles = "admin,user")]
        public JsonResult AddApply(string di_EqId, string a_Resion)
        {
            di_EqId = di_EqId.Trim();
            if (di_EqId == null || a_Resion == null) {
                return Json(new { state = "error", msg = "设备编号和原因不能为空" },JsonRequestBehavior.AllowGet);
            }
            if ((DB.DecivesInfo.FirstOrDefault(u=>u.di_EqId==di_EqId))==null) {
                return Json(new { state = "error", msg = "设备编号不存在" }, JsonRequestBehavior.AllowGet);
            }
            if ((DB.RepairApply.FirstOrDefault(u=>u.di_EqId==di_EqId&&u.ra_Status=="未完成"&&u.ra_Approval!=-1)) != null)
            {
                return Json(new { state = "error", msg = "该设备已经在申请队列中" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var userdata = (System.Web.HttpContext.Current.User as MyUserDataPrincipal).userdata;
                var data = new RepairApply { e_ID = userdata.e_id, di_EqId = di_EqId, ra_Time = DateTime.Now, ra_Resion = a_Resion, ra_Approval = 0, ra_AllocationPerson = 0, ra_Status = "未完成",d_ID=userdata.d_ID};
                RepairApplyServices.AddRecorde(data);
                return Json(new { state = "success", msg = "申请成功" }, JsonRequestBehavior.AllowGet);
            }
           catch(Exception ex){
               return Json(new { state = "error", msg =ex.ToString() }, JsonRequestBehavior.AllowGet);
           }         
                
        }
    }
}
