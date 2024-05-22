using Interior_Quotation_Ecommerce.MongoDB.Entities;

namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class ProductsDTO : AbstractDTO
    {
        public long ContractorId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }
    }
}