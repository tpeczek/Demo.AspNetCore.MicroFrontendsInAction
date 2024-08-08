using Microsoft.AspNetCore.Mvc;
using Demo.AspNetCore.MicroFrontendsInAction.Inspire.Models;

namespace Demo.AspNetCore.MicroFrontendsInAction.Inspire.Controllers
{
    public class RecommendationsController : Controller
    {
        private static readonly IReadOnlyDictionary<string, RecommendationViewModel> _recommendations = new Dictionary<string, RecommendationViewModel>
        {
            { "eicher", new RecommendationViewModel("porsche", "fendt") },
            { "fendt", new RecommendationViewModel("eicher", "porsche") },
            { "porsche", new RecommendationViewModel("fendt", "eicher") }
        };

        public IActionResult Recommendation(string id)
        {
            return View(_recommendations[id.ToLowerInvariant()]);
        }

        public IActionResult RecommendationFragment(string id)
        {
            return View(_recommendations[id.ToLowerInvariant()]);
        }
    }
}
