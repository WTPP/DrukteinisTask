namespace Models.Builders
{
    public sealed class ProductBuilder
    {
        public ProductBuilder() => _product = new ProductModel();

        public static implicit operator ProductModel(ProductBuilder builder) => builder._product;

        private readonly ProductModel _product;

        public ProductBuilder SetId(int id)
        {
            _product.Id = id;
            return this;
        }

        public ProductBuilder SetName(string name)
        {
            _product.Name = name;
            return this;
        }

        public ProductBuilder SetDescription(string description)
        {
            _product.Description = description;
            return this;
        }

        public ProductBuilder SetPrice(double price)
        {
            _product.Price = price;
            return this;
        }
    }
}
