namespace Demo.AspNetCore.MicroFrontendsInAction.Inspire.Models
{
    public class RecommendationViewModel
    {
        public string[] ProductsIds { get; }

        public RecommendationViewModel(params string[] productsIds)
        {
            ProductsIds = productsIds;
        }
    }
}
