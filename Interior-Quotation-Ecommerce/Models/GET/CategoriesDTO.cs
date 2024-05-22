namespace Interior_Quotation_Ecommerce.Models.GET
{
    public class CategoriesDTO : AbstractDTO
    {
        public string? Name { get; set; }
        public List<ConstructsDTO>? constructsViewList { get; set; }

    }
}
