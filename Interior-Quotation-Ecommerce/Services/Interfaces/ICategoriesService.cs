using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface ICategoriesService
    {
        IEnumerable<CategoriesDTO> GetCategories();
        CategoriesDTO GetCategoryById(int id);

        CategoriesDTO AddCategory(CategoriesDTO categoryView);

        CategoriesDTO UpdateCategory(int id, CategoriesDTO updatedCategoryView);

        CategoriesDTO DeleteCategory(int id);
    }
}
