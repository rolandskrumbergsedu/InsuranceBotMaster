using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuranceBotMaster.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error($"Hit on Index @ {DateTime.Now}");
            return View();
        }

        public ActionResult Instructions()
        {
            return View();
        }
    }
}