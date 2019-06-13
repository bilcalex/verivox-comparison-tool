namespace CompareApi.Models
{
    public enum ProductType
    {
        Basic,
        Packaged
    }
    public class ProductItem
    {
        public long Id { get; set; }
        public ProductType Type { get; set; }
        public int Consumption { get; set; }
        public double AnnualCost { get; set; }

    }
}