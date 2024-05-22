namespace Interior_Quotation_Ecommerce.Entities
{
    public class Accounts : BaseEntity
    {
        //Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
        public Role? AccountRole { get; set; }
        public enum Role
        {
            ADMIN,
            CONTRACTOR,
            CUSTOMER
        } 

        //Relationship
        public IEnumerable<Tokens>? Tokens { get; set; }
        public Contractors? Contractor { get; set; }
        public Customers? Customer { get; set; }
    }
}
