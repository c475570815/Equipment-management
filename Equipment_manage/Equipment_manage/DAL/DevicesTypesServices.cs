using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Equipment_manage.DAL
{
    public class DevicesTypesServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<DevicesTypes> table = DB.DevicesTypes;
        public static int NameGetId(string name)
        {
            return table.FirstOrDefault(p => p.dt_Name == name).dt_ID;
        }
        public static List<DevicesTypes> GetAllRecord()
        {
            return table.ToList();
        }
        public static string IdGetName(int id)
        {
            return table.FirstOrDefault(p => p.dt_ID == id).dt_Name;
        }
    }
}