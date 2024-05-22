namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class RequestDetailsDTO
    {
        public long RequestId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductsDTO? Products { get; set; }
    }
}
