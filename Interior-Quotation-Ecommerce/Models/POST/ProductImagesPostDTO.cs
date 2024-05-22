namespace Interior_Quotation_Ecommerce.Models.POST
{
    public class ProductImagesPostDTO
    {
        public long ProductId { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
