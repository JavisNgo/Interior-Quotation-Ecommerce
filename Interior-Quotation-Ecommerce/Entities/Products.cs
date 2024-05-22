namespace Interior_Quotation_Ecommerce.Entities
{
    public class Products : BaseEntity
    {
        //Properties
        public string? Code { get; set; }
        public long ContractorId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }

        //Relationship
        public Contractors? Contractor { get; set; }
        public ICollection<ConstructProducts>? ConstructProducts { get; set; }
        public ICollection<RequestDetails>? RequestDetails { get; set; }
    }
}
