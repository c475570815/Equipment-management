using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Equipment_manage.Models;
using System.Web.Script.Serialization;
using PagedList;
using Newtonsoft.Json.Linq;
using Equipment_manage.Models.Authen;
using Equipment_manage.DAL;

namespace Equipment_manage.Controllers
{
    public class EquipmentController : Controller
    {
        [MyAuthorize(Roles = "admin,user,downer,wowner,serviceman")]
        public ActionResult Equipment()
        {
            return View();
        }
        /// <summary>
        /// 做设备信息的查询和分页
        /// </summary>
        /// <param name="pageSize">页面显示的最大记录条数</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="id">查询的设备编号</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize(Roles = "admin,user,downer,wowner,serviceman")]
        public JsonResult GetEquipment(int pageSize, int pageNumber, string id="", string sortOrder="", string sortName="", string keyword = "")
        {
            var date = DecivesInfoServices.GetWhere(keyword, id);
            var total = date.Count();
            var rows = DecivesInfoServices.Sort(date, sortOrder, sortName).ToPagedList(pageNumber, pageSize).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.DenyGet);
        }
        [HttpGet]
        public string Add(string date,string typename)
        {
            string result = "";
            DecivesInfo recorde = JSONHelper.ParseFormByJson<DecivesInfo>(date);
            if (recorde.di_EqId == "")
                return "非法提交";
            if (DecivesInfoServices.Repetition(recorde.di_EqId))
                return "重复的设备编号";
            try
            {
                recorde.dt_ID = DevicesTypesServices.NameGetId(typename);
                DecivesInfoServices.AddRecorde(recorde);
                result = "添加成功";
            }
            catch (Exception e)
            {
                result = "添加失败:"+e.ToString();
            }
            return result;
        }
        [HttpGet]
        public string Alert(string date, string typename)
        {
            string result = "";
            try
            {
                DecivesInfo a = JSONHelper.ParseFormByJson<DecivesInfo>(date);
                a.dt_ID = DevicesTypesServices.NameGetId(typename);
                result=DecivesInfoServices.Alert(a, a.di_EqId);
            }
            catch
            {
                return "修改失败";
            }
            return result;        
        }
        [HttpGet]
        public string Delete(string id)
        {
           return DecivesInfoServices.Delete(id);
        }
    }
}