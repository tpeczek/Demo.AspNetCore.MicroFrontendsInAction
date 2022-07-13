using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.MicroFrontendsInAction.Inspire.Controllers
{
    public class RecommendationsController : Controller
    {
        public IActionResult Eicher()
        {
            return View();
        }

        public IActionResult Fendt()
        {
            return View();
        }

        public IActionResult Porsche()
        {
            return View();
        }
    }
}
