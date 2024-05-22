using Interior_Quotation_Ecommerce.MongoDB.Entities;

namespace Interior_Quotation_Ecommerce.MongoDB.Interfaces
{
    public interface IProductImagesRepository
    {
        void Create(ProductImages productImages);
        void CreateMany(IEnumerable<ProductImages> productImages);
        List<ProductImages> GetAll();
        void Delete(object id);
        void Update(ProductImages productImages);
    }
}
