using Interior_Quotation_Ecommerce.Entities;

namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class ConstructProductsDTO : AbstractDTO
    {
        public long ProductId { get; set; }
        public long ConstructId { get; set; }
        public int Quantity { get; set; }
        public ProductsDTO? Product { get; set; }
        public ConstructsDTO? Construct { get; set; }
    }
}