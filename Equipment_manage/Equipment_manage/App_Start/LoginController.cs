using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Equipment_manage.Models;
using Equipment_manage.Models.Authen;
using System.Web.Security;

namespace Equipment_manage.Controllers
{
   
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        Equipment DB = new Equipment();
        public ActionResult Index()
        {
            return View();
        }
       [HttpPost]
        public JsonResult CheckLogin(Users  user) {
           //传入为空
           if(user.e_ID.ToString()==null||user.u_Password==null){
               return Json(new { state = "error" }, JsonRequestBehavior.AllowGet);
           }
           //查询
           var User = DB.Users.FirstOrDefault(u => u.e_ID == user.e_ID && u.u_Password == user.u_Password);
           //成功
           if (User != null) {
               //查询数据
               var userinfo = DB.Employees.FirstOrDefault(n => n.e_ID == user.e_ID);
               //写入数据
               var userData = new MyUserDataPrincipal { userdata = new UserData {e_id=userinfo.e_ID,e_Name=userinfo.e_Name,d_ID=userinfo.d_ID,u_Type=User.u_Type } };
               //把写入的数据写入cookie
               MyFormsAuthentication<MyUserDataPrincipal>.SetAuthCookie(userinfo.e_Name, userData,false);

               return Json(new { state = "success" }, JsonRequestBehavior.AllowGet);
           }
           //失败
           return Json(new { state = "error" }, JsonRequestBehavior.AllowGet);
        }
       [HttpGet]
       public ActionResult CheckLoginOut() {
           FormsAuthentication.SignOut();
           return RedirectToAction("index");
       }
	}
}