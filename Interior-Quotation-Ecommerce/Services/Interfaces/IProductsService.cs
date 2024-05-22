using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<ProductsDTO> GetAll();
        ProductsDTO GetById(long id);
        ProductsDTO AddProduct(ProductsPostDTO productsPostDTO);
        ProductsDTO UpdateProduct(ProductsPostDTO productsPostDTO);
        bool IsDeleteProduct(long id);
    }
}
