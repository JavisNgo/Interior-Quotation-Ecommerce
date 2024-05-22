namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class ContractorsDTO : AbstractDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public long AccountId { get; set; }
    }
}
