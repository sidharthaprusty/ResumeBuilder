using Resume_Builder.Models;
using System.Linq;
using System.Web.Mvc;

namespace Resume_Builder.Controllers
{
    public class HomeController : Controller
    {
        Resume_BuilderEntities db = new Resume_BuilderEntities();

        public ActionResult Index()
        {
            var resumes = db.Resumes.ToList();
            return View(resumes);
        }

        [HttpPost]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FillDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FillDetails(FormCollection values, int id)
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}