namespace Interior_Quotation_Ecommerce.Entities
{
    public class Messages : BaseEntity
    {
        //Properties
        public string? Content { get; set; }
        public long CustomerId { get; set; }
        public long ContractorId { get; set; }

        //Relationship
        public Customers? Customer { get; set; }
        public Contractors? Contractor { get; set; }
    }
}