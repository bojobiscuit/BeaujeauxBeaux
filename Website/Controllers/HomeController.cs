using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Halp()
        {
            return View();
        }

        public ActionResult Requests()
        {
            RequestSubmitModel requestSubmit = new RequestSubmitModel();
            return View(requestSubmit);
        }

        [HttpPost]
        public ActionResult Requests(RequestSubmitModel requestSubmit)
        {
            requestSubmit.Submitted = false;
            requestSubmit.AddToDB();
            requestSubmit.GetSubmissions();
            if(requestSubmit.Submitted)
            {
                requestSubmit.Description = "";
                requestSubmit.SelectedRequestId = null;
            }
            return View(requestSubmit);
        }

    }
}