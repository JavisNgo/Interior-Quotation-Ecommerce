namespace Interior_Quotation_Ecommerce.Models.POST
{
    public class ConstructsPostDTO
    {
        public string? Code { get; set; }
        public long ContractorId { get; set; }
        public long CategoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? EstimatedPrice { get; set; }
        public bool? Status { get; set; }
        public List<ConstructProductsPostDTO>? constructProductsPost { get; set; }
    }
}
