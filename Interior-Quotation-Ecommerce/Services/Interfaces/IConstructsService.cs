using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface IConstructsService
    {
        ConstructsDTO GetConstructById(long id);
        List<ConstructsDTO> Get();
        ConstructsDTO CreateConstruct(ConstructsPostDTO constructsView, List<ConstructImagesPostDTO> fileUploads);
        ConstructsDTO UpdateConstruct(ConstructsPostDTO constructsView, List<ConstructImagesPostDTO> fileUploads);
        bool IsDeleteConstruct(long deleteConstructId);

    }
}
