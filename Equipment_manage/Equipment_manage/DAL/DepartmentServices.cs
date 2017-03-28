using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Equipment_manage.DAL
{
    public class DepartmentServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<Department> table = DB.Department;
        public static List<Department> GetAllRecorde()
        {
            return table.ToList();
        }
        public static int NameGetId(string name)
        {
            return table.FirstOrDefault(p => p.d_Name == name).d_ID;
        }
        public static string IdGetName(int id)
        {
            return table.FirstOrDefault(p => p.d_ID == id).d_Name;
        }
    }
}