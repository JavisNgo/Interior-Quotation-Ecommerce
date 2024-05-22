namespace Interior_Quotation_Ecommerce.Entities
{
    public class Contracts : BaseEntity
    {
        public long RequestId { get; set; }
        public string? ContractUrl { get; set; }
        public enum RequestsStatusEnum
        {
            DONE,
            PENDING,
            REJECTION,
        }

        public Requests? Request { get; set; }
    }
}