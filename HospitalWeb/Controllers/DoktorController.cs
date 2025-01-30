using Microsoft.AspNetCore.Mvc;

namespace HospitalWeb.Controllers
{
    public class DoktorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
