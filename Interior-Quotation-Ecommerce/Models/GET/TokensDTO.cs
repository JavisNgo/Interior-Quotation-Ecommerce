namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class TokensDTO
    {
        public long AccountId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
