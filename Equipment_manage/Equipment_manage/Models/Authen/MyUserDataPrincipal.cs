using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;
using Equipment_manage.Models;
using Equipment_manage.Models.Authen;

namespace Equipment_manage.Models.Authen
{
    public class MyUserDataPrincipal:IPrincipal
    {
        Equipment DB = new Equipment();
        public IIdentity Identity { get; set;}
        //用户数据
        public UserData  userdata { get; set; }
        //当使用Authorize特性时，会调用改方法验证角色 
        public bool IsInRole(string role)
        {
            if (role == "*")
            {
                return true;
            }
            try
            {
                var roles = role.Split(',');
                var user = DB.Users.Where(u => u.e_ID == userdata.e_id).First();
                var userroles = user.u_Type.Split(',');
                return (from s in roles from userrole in userroles where s.Equals(userrole) select s).Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

     
    }
}