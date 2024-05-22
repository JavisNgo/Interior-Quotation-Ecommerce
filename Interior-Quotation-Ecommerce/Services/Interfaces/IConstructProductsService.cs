using Interior_Quotation_Ecommerce.Models.POST;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface IConstructProductsService
    {
        bool IsUpdateQuantity(ConstructProductsPostDTO constructProductsPostDTO);
    }
}
