using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.MicroFrontendsInAction.Checkout.Controllers
{
    public class FragmentsController : Controller
    {
        public IActionResult Buy(string sku, string edition)
        {
            IDictionary<string, string> model = new Dictionary<string, string>
            {
                { "Sku", sku },
                { "Edition", edition }
            };

            return View("Buy", model);
        }
    }
}