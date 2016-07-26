using System.Web.Mvc;
using JR.Common;
using JR.Common.MVC;
using SZ.Aisino.CMS.Models;
using SZ.Aisino.CMS.DbEntity;

namespace SZ.Aisino.CMS.Controllers
{
    public class EsDPMapController : Controller
    {

        public ActionResult Index()
        {

 
            return View();
        }
        public ActionResult Extents()
        {

 
            return View();
        }

        #region DP3ASingleton

        /// DP3ASingleton
        public ActionResult DP3_1_Singleton()
        {


            return View();
        }
        #endregion
        #region DP4_1_Observer
        /// DP4_1_Observer
        public ActionResult DP4_1_Observer()
        {


            return View();
        }
        #endregion

        #region ESDP5_Strategy
        /// ESDP5_Strategy

        public ActionResult ESDP5_Strategy_OOP()
        {
            return View();
        }
        public ActionResult ESDP5_Strategy_Func()
        {
            return View();
        }
        public ActionResult ESDP5_Strategy_Animate()
        {
            return View();
        }
        public ActionResult ESDP5_Strategy_FormAuth()
        {
            return View();
        }
        #endregion

    }
}