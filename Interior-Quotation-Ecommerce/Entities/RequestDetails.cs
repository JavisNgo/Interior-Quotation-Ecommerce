namespace Interior_Quotation_Ecommerce.Entities
{
    public class RequestDetails : BaseEntity
    {
        //Properties
        public long RequestId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }

        //Relationship
        public Requests? Request { get; set; }
        public Products? Product { get; set; }
    }
}