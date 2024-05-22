using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.MongoDB.Entities;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface IConstructImagesService
    {
        List<ConstructImages> GetConstructImages();
        List<ConstructImages> GetConstructImagesByConstructId(long ConstructId);
        bool IsDeleteImageByConstructId(long ConstructId);
        ConstructImages CreateConstructImages(ConstructImagesPostDTO fileUpload);
        ConstructImages UpdateConstructImages(ConstructImagesPostDTO fileUpload);
        
    }
}
