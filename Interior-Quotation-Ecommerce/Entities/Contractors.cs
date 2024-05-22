namespace Interior_Quotation_Ecommerce.Entities
{
    public class Contractors : BaseEntity
    {
        //Properties
        public long AccountId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }

        //Relationship
        public Accounts? Account { get; set; }
        public ICollection<Products>? Products { get; set; }
        public ICollection<Constructs>? Constructs { get; set; }
        public ICollection<Requests>? Requests { get; set; }
        public ICollection<Messages>? Messages { get; set; }
    }
}
