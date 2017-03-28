using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Equipment_manage.DAL
{
    public class EmployeesServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<Employees> table = DB.Employees;
        private static IQueryable<Employees> InitialDate = null;
        public static IQueryable<Employees> GetWhere(string keyword, int? id)
        {
            IQueryable<Employees> a;
                int? d_ID;
                try
                {
                    d_ID = Convert.ToInt32(keyword);
                }
                catch
                {
                    d_ID = null;
                }
                a = table.Where(q => q.e_Name.Contains(keyword) ? true : //名
                     q.e_Gender.Contains(keyword)//性别
                    || q.e_Mobile == keyword //电话
                    || d_ID != null ? q.d_ID == d_ID : false)
                   .Where(p => id != null ? id == p.e_ID : true);
            return a;
        }
        public static List<string> GetAllPeople()
        {
            return table.Select(p => p.e_Name).ToList();
        }
        public static IQueryable<Employees> Sort(IQueryable<Employees> date, string sortOrder, string sortName)
        {
                InitialDate = date.OrderBy(i => i.e_ID);
            if (sortName != "" && InitialDate != null)
            { 
                if (sortOrder != "desc")
                    switch (sortName)
                    {
                        case "d_ID": InitialDate = InitialDate.OrderBy(P => P.d_ID); break;
                        case "e_Gender": InitialDate = InitialDate.OrderBy(P => P.e_Gender); break;
                        default:  break;
                    }
                else
                    switch (sortName)
                    {
                        case "d_ID": InitialDate = InitialDate.OrderByDescending(P => P.d_ID); break;
                        case "e_Gender": InitialDate = InitialDate.OrderByDescending(P => P.e_Gender); break;
                        default:break;
                    }            
            }
            return InitialDate;
        }
        public static string Alert(Employees a)
        { 
               var record = DB.Employees.FirstOrDefault(p => p.e_ID == a.e_ID);
                if (record.Equals(a))
                {
                    return "您没有做修改";
                }
                try
                {
                    record.clone(a);
                    DB.SaveChanges();
                    return "修改成功";
                }
                catch {
                    return "修改失败";
                }
        }
        public static int Delete(int id)
        {
            var record=table.FirstOrDefault(a => a.e_ID == id);
            DB.Employees.Remove(record);
            return  DB.SaveChanges();
        }
        public static bool Repetition(int id)
        {
            return table.Where(p => p.e_ID == id).Count() == 0 ? false : true;
        }
        public static void AddRecorde(Employees recorde)
        {
            table.Add(recorde);
            DB.SaveChanges();
        }
        public static Employees IdGetRecord(int id)
        {
            return  table.FirstOrDefault(p => p.e_ID == id);
        }
        public static string IdGetName(int id)
        {
            return table.FirstOrDefault(p => p.e_ID == id).e_Name;
        }
        public static IQueryable<Employees> GetAppointWhere(int id)
        {
            return table.Where(p => p.d_ID == id);
        }
    }
}