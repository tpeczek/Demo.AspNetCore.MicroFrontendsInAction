using Microsoft.AspNetCore.Mvc;
using Demo.AspNetCore.MicroFrontendsInAction.Decide.Models;

namespace Demo.AspNetCore.MicroFrontendsInAction.Decide.Controllers
{
    public class ProductsController : Controller
    {
        private static readonly IReadOnlyDictionary<string, ProductViewModel> _products = new Dictionary<string, ProductViewModel>
        {
            { "eicher", new ProductViewModel("eicher", "Eicher Diesel 215/16") },
            { "fendt", new ProductViewModel("fendt", "Fendt F20 Dieselroß") },
            { "porsche", new ProductViewModel("porsche", "Porsche-Diesel Master 419") }
        };

        public IActionResult Product(string id)
        {
            return View(_products[id.ToLowerInvariant()]);
        }
    }
}
