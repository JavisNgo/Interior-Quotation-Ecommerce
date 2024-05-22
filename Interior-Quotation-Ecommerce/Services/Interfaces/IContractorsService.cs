using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface IContractorsService
    {
        List<ContractorsDTO> GetAll();
        ContractorsDTO GetById(int id);
        ContractorsDTO CreateContractor(ContractorsPostDTO contractorsPostDTO);
        ContractorsDTO UpdateContractor(long id, ContractorsPostDTO contractorsPostDTO);
        bool IsChangedStatusContractorById(long id);
    }
}
