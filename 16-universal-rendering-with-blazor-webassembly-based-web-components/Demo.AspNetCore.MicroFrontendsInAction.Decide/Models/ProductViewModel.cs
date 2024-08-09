namespace Demo.AspNetCore.MicroFrontendsInAction.Decide.Models
{
    public class ProductViewModel
    {
        public string Id { get; }

        public string Name { get; }

        public ProductViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
