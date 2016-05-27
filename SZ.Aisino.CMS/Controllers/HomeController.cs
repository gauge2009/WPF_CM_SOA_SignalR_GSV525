using System.Web.Mvc;
using JR.Common;
using JR.Common.MVC;
using SZ.Aisino.CMS.Models;
using SZ.Aisino.CMS.DbEntity;

namespace SZ.Aisino.CMS.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index( )
        {

            //return Content("Alayaz  SOA  Service  Activating ......");

            return View();
        }
      

    }
}