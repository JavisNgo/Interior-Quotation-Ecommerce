using Interior_Quotation_Ecommerce.MongoDB.Entities;

namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class ConstructsDTO : AbstractDTO
    {
        public long ContractorId { get; set; }
        public long CategoryId { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? EstimatedPrice { get; set; }
        public bool? Status { get; set; }
        public ContractorsDTO? Contractor { get; set; }
        public CategoriesDTO? CategoriesView { get; set; }
        public List<ConstructProductsDTO>? constructProductsViews { get; set; }

    }
}