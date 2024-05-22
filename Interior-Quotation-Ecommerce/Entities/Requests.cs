using System.Diagnostics.Contracts;

namespace Interior_Quotation_Ecommerce.Entities
{
    public class Requests : BaseEntity
    {
        //Properties
        public long CustomerId { get; set; }
        public long ContractorId { get; set; }
        public string? Notes { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? TimeOut { get; set; }
        public RequestsStatusEnum? Status { get; set; }
        public enum RequestsStatusEnum
        {
            PENDING,
            ACCEPTED,
            COMPLETED,
            SIGNED,
            DEPOSITED,
            REJECTED,
        }

        //Relationship
        public ICollection<RequestDetails>? RequestDetails { get; set; }
        public Customers? Customer { get; set; }
        public Contractors? Contractor { get; set; }
        public Contracts? Contract { get; set; }
    }
}