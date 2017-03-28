using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Equipment_manage.Models;
using Equipment_manage.DAL;

namespace Equipment_manage.Controllers
{
    public class EmployeeController : Controller
    {
        Equipment DB = new Equipment();
        //
        // GET: /Employee/
        public ActionResult Employee()
        {
            return View();
        }
        public JsonResult GetEmployee(int pageSize, int pageNumber, string keyword = "", int? id = null, string sortOrder = "", string sortName = "")
        {
            var date= EmployeesServices.GetWhere(keyword, id);
            var total = date.Count();
            var rows = EmployeesServices.Sort(date, sortOrder, sortName).ToPagedList(pageNumber, pageSize).ToList();
            return Json(new { total = total, rows = rows },JsonRequestBehavior.AllowGet);
        }
        public string  Delete(string del_id)
        {
            string[] id = del_id.Split(',');
              int p = 0;
            try
            {
                for (int i = 0; id[i] != ""; i++)
                {
                    var del = int.Parse(id[i]);
                    p += EmployeesServices.Delete(del);
                }
                return "已经删除"+p+"条记录";
            }
            catch
            {
                var error = id.Length - p-1;
                return "删除失败" + error + "条记录";
            }
        }
        [HttpGet]
        public string Add(string date, string Department)
        {
            string result = "";
            Employees a = JSONHelper.ParseFormByJson<Employees>(date);
            a.d_ID = DepartmentServices.NameGetId(Department);
            if (a.e_ID == 0)
                return "非法提交(id为空)";
            else
            {
                if (EmployeesServices.Repetition(a.e_ID))
                    return "非法提交(重复的id)";
            }
            try
            {
                EmployeesServices.AddRecorde(a);
                Users user = new Users();
                user.e_ID = a.e_ID;
                user.u_Password = "1111";
                user.u_Type = "user";
                UsersServices.Add(user);
                result = "添加成功";
                
            }
            catch (Exception ex)
            {
                result = "添加失败:"+ex.ToString();
            }
            return result;
        }
        [HttpGet]
        public string Alert(string date, string Department)
        {
            string result = "";
            try
            {
                Employees a = JSONHelper.ParseFormByJson<Employees>(date);
                a.d_ID = DepartmentServices.NameGetId(Department);
                result= EmployeesServices.Alert(a);
            }
            catch 
            {
                return "修改失败";
            }
            return result;
        }
	}
}