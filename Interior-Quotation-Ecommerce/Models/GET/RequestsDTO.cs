namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class RequestsDTO
    {
        public long CustomerId { get; set; }
        public long ContractorId { get; set; }
        public string? Code { get; set; }
        public string? Note { get; set; }
        public double? TotalPrice { get; set; }
        public enum RequestsStatusEnum
        {
            PENDING,
            ACCEPTED,
            COMPLETED,
            SIGNED,
            DEPOSITED,
            REJECTED,
        }
        public List<RequestDetailsDTO>? RequestDetailViews { get; set; }
    }
}
