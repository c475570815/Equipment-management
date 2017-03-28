using Equipment_manage.Models;
using Equipment_manage.Models.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Equipment_manage.DAL;

namespace Equipment_manage.Controllers
{
    public class MaintenanceController : Controller
    {
        public Equipment DB = new Equipment();
        #region 设备审批
        //
        // GET: /Maintenance/
         [MyAuthorize(Roles = "admin,downer")]
        public ActionResult MaintenanceApply()
        {
            return View();
        }
        [HttpGet]
        [MyAuthorize(Roles = "admin,downer")]
        public JsonResult GetMaintenanceApply(int pageSize, int pageNumber,string sortOrder, string sortName)
        {
            var date = RepairApplyServices.GetWhere();
            var total = date.Count();
            var rows = RepairApplyServices.Sort(date, sortOrder, sortName).ToPagedList(pageNumber, pageSize).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string Pass(string pass_Id)
        {
            
            string[]id = pass_Id.Split(',');
            int p = 0;
            try
            {
                for (int i = 0; id[i] != ""; i++)
                {
                    int j=Convert.ToInt32(id[i]);
                    p += RepairApplyServices.PassApply(j);
                }
                return "已经通过"+p+"条申请";
            }
            catch
            {
                var error = id.Length - p;
                return "通过失败"+error+"条";
            }
               
        }
        public string Refuse(string refuse_Id)
        {
            var date = DB.RepairApply;
            try
            {
                int id = Convert.ToInt32(refuse_Id);
                RepairApplyServices.RefuseApply(id);
                return "已经拒绝该申请";
            }
            catch
            {
                return "拒绝失败";
            }
        }

        [HttpGet]
        public JsonResult Getinformation(int id)
        {
            var date = RepairApplyServices.IdGetRecord(id);
            var e_id=date.e_ID;
            var people = EmployeesServices.IdGetRecord(e_id);
            var apply_name = people.e_Name;//申请者姓名
            var department_id = people.d_ID;
            var department_name = DepartmentServices.IdGetName(department_id);
            var di_id=date.di_EqId;
            var recd = DecivesInfoServices.IdGetRecorde(di_id);
            var Dinf_name = recd.di_Name;//设备名称
            var type_id = recd.dt_ID;
            var Dinf_type = DevicesTypesServices.IdGetName(type_id);
            return Json(new { apply_name = apply_name, department_name = department_name, Dinf_type = Dinf_type, Dinf_name = Dinf_name}, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 工单表

        [MyAuthorize(Roles = "admin,user,downer,wowner,serviceman")]
        public  ActionResult MaintenanceForm()
        {
            return View();
        }
        [HttpGet]
        [MyAuthorize(Roles = "admin,user,downer,wowner,serviceman")]
        public  JsonResult GetMaintenanceForm(int pageSize, int pageNumber,string sortOrder, string sortName)
        {
            var date = RepairDetailsServices.GetWhere();
            List<RepairDetails> result = RepairDetailsServices.Sort(date, sortOrder, sortName).ToPagedList(pageNumber, pageSize).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].e_ID == 0)//未派人时工单表中e_id等于0
                    continue;
                //把id转换为姓名
                int id = result[i].e_ID;
                result[i].name = EmployeesServices.IdGetName(id);
                //把id转换为姓名
                int  ra_id=(int)result[i].ra_ID;
                result[i].over = RepairApplyServices.IdGetStatus(ra_id);
                //查询完成状态
            }
            var total = date.Count();
            var rows = result;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string Maintenanceinf(int ra_id)
        {
            Maintenanceinf inf=new Models.Maintenanceinf();
            var Arecord = RepairApplyServices.IdGetRecord(ra_id);//在申请表中找到记录
            inf.ra_ID =ra_id;//申请编号
            inf.ra_people = EmployeesServices.IdGetName(Arecord.e_ID);//申请人
            inf.ra_eqid = int.Parse(Arecord.di_EqId);//设备编号
            inf.ra_time = Arecord.ra_Time;//开始申请时间
            inf. ra_pass = Arecord.ra_Approval;//是否通过审批
            if(inf.ra_pass==-1)
                return JSONHelper.GetJSON<Maintenanceinf>(inf);
            inf. ra_AllocationPerson = Arecord.ra_AllocationPerson;//是否派人
            inf. ra_paas_time = Arecord.ra_ApprovalTime;//通过时间
            var Drecord = RepairDetailsServices.Ra_idGetRecord(ra_id);//在工单表中找到记录
            var e_Id = Drecord.e_ID;
            inf. re_people=""; 
            inf. ra_advice="";
            inf.rd_endtime=null;
            if (Arecord.ra_Approval != -1)
            {
                if (e_Id != 0)
                 inf.re_people = EmployeesServices.IdGetName(e_Id);//维修人员
                 inf.ra_advice = Drecord.rd_Advice;//反馈信息
                 inf.rd_endtime = Drecord.rd_EndTime;//维修结束时间
            }
            return JSONHelper.GetJSON<Maintenanceinf>(inf);
        }
        [HttpGet]
        public string AddMaintenanceFeedback(string rd_Advice, int ra_id)
        {
            try
            {
                var Drecord = RepairApplyServices.IdGetRecord(ra_id);
                if (RepairApplyServices.OverStatus(ra_id)
                    && DecivesInfoServices.ToUseStatus(Drecord.di_EqId) 
                    && RepairDetailsServices.OverBack(ra_id, rd_Advice))
                    return "提交成功";
                else
                    return "提交失败";
            }
            catch
            {
                return "提交失败";
            }
        }
        #endregion
        #region 派人表
        [MyAuthorize(Roles = "admin,wowner")]
        public ActionResult MaintenanceAppoint()
        {
            return View();
        }
         [MyAuthorize(Roles = "admin,wowner")]
        public JsonResult GetMaintenanceAppoint(int pageSize, int pageNumber, string sortOrder, string sortName)
        {
            var date = RepairApplyServices.GetAppointWhere();
            var result = RepairApplyServices.Sort(date, sortOrder, sortName).ToPagedList(pageNumber, pageSize).ToList();
            for (int i = 0; i < result.Count; i++)
            {

                string di_EqId = result[i].di_EqId;
                var placename = DecivesInfoServices.IdGetRecorde(di_EqId);
                result[i].place = placename.di_Position;//设备所在地

                int type_id = placename.dt_ID;
                var typename = DevicesTypesServices.IdGetName(type_id);
                result[i].typename = typename;//设备类型名

                int name_id = result[i].e_ID;
                var applyname = EmployeesServices.IdGetRecord(name_id);
                result[i].applyname = applyname.e_Name;//申请人姓名
                result[i].applyphnoe = applyname.e_Mobile;//申请人电话
            }
            var total = date.Count();
            var rows = result;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  派遣人员表
        [HttpGet]
        public JsonResult GetAppointPeople(int pageNumber, int pageSize)
        {
            var dateDP = DB.Department;
            var dateE = DB.Employees;
            var d_id = DepartmentServices.NameGetId("维修部");
            var date = EmployeesServices.GetAppointWhere(d_id).OrderBy(i => i.e_ID);
            var result = date.ToPagedList(pageNumber, pageSize).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                var e_id = result[i].e_ID;
                result[i].work_number = RepairDetailsServices.GetWorkNumber(e_id);
            }
            var total = date.Count();
            var rows = result;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string  Assign(int people_id,int apply_id)
        {
            try
            {
                RepairDetailsServices.AlertAppointPeople(people_id, apply_id);//修改工单表状态
                RepairApplyServices.AlertApplyStatus(apply_id);//修改申请表状态
                return "指派成功";
            }
            catch 
            {
                return "指派失败";
            }
        }
        #endregion
        #region 维修反馈表
        //
        // GET: /Maintenance/MaintenanceFeedback
        public ActionResult MaintenanceFeedback()
        {
            return View();
        }
        #endregion
    }
}