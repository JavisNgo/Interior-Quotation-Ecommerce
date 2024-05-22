using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.MongoDB.Entities;

namespace Interior_Quotation_Ecommerce.MongoDB.Interfaces
{
    public interface IConstructImagesRepository
    {
        void Create(ConstructImages constructImages);
        void CreateMany(IEnumerable<ConstructImages> constructImages);
        ConstructImages GetById(string id);
        List<ConstructImages> GetAll();
        void Delete(object id);
        void Update(ConstructImages updateConstructImages);
    }
}
