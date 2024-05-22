namespace Interior_Quotation_Ecommerce.Entities
{
    public class Tokens : BaseEntity
    {
        //Properties
        public long AccountId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiredDate { get; set; }

        //Relationship
        public Accounts? Account { get; set; }
    }
}