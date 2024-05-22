namespace Interior_Quotation_Ecommerce.Entities
{
    public class ConstructProducts : BaseEntity
    {
        //Propertiess
        public long ProductId { get; set; }
        public long ConstructId { get; set; }
        public int Quantity { get; set; }

        //Relatioship
        public Products? Product { get; set; }
        public Constructs? Construct { get; set; }
    }
}