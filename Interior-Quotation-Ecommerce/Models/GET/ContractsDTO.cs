namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class ContractsDTO : AbstractDTO
    {
        public long RequestId { get; set; }
        public string? ContractUrl { get; set; }
        public string? Progress { get; set; }
        public int? Status { get; set; }

    }
}
