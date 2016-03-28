using MaBiblio.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace MaBiblio.Controllers
{
    public class HomeController : Controller
    {
        private BiblioContext db = new BiblioContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "UQAM. Bureau 12345, Tel 303 123 0101 Poste 888";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}