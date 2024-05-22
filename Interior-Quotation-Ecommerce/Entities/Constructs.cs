namespace Interior_Quotation_Ecommerce.Entities
{
    public class Constructs : BaseEntity
    {
        //Properties
        public long ContractorId { get; set; }
        public long CategoryId { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? EstimatedPrice { get; set; }
        public bool? Status { get; set; }


        //Relationship
        public Contractors? Contractor { get; set; }
        public ICollection<ConstructProducts>? ConstructProducts { get; set; }
        public Categories? Category { get; set; }
    }
}
