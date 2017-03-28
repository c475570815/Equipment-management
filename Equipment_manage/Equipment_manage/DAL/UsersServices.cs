using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Equipment_manage.DAL
{
    public class UsersServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<Users> table = DB.Users;
        public static bool Add(Users user)
        {
            try {
                table.Add(user);
                DB.SaveChanges();
                return true; }
            catch { return false; }
        }
    }
}