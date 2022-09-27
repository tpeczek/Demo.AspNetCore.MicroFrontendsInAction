namespace Demo.AspNetCore.MicroFrontendsInAction.Decide.Frontend.Models
{
    internal sealed class Product
    {
        public string Name { get; private set; }

        public string Image { get; private set; }

        public static IReadOnlyDictionary<string, Product> Products { get; } = new Dictionary<string, Product>
        {
            { "porsche", new Product("Porsche-Diesel Master 419", "porsche.svg") },
            { "fendt", new Product("Fendt F20 Dieselroß", "fendt.svg") },
            { "eicher", new Product("Eicher Diesel 215/16", "eicher.svg") }
        };

        public Product(string name, string image)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }
    }
}