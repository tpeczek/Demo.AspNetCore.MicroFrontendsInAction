using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.MicroFrontendsInAction.Decide.Controllers
{
    public class ProductsController : Controller
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
