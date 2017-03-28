using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Equipment_manage.Models.Authen;
namespace Equipment_manage.Controllers
{
    public class IndexController : Controller
    {
        [MyAuthorize(Roles = "*")]
        public ActionResult Index()
        {
              return View();
        }
	}
}