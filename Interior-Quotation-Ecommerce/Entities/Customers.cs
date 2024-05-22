namespace Interior_Quotation_Ecommerce.Entities
{
    public class Customers : BaseEntity
    {
        //Properties
        public long? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        //Relationship
        public Accounts? Account { get; set; }
        public ICollection<Messages>? Messages { get; set; }
        public ICollection<Requests>? Requests { get; set; }
    }
}
