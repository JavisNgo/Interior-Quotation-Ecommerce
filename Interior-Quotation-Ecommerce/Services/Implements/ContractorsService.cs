using AutoMapper;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class ContractorsService : IContractorsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContractorsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ContractorsDTO CreateContractor(ContractorsPostDTO contractorsPostDTO)
        {
            throw new NotImplementedException();
        }

        public List<ContractorsDTO> GetAll()
        {
            try
            {
                var contractors = _unitOfWork.ContractorsRepository.Get().ToList();
                return _mapper.Map<List<ContractorsDTO>>(contractors);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ContractorsDTO GetById(int id)
        {
            try
            {
                var contractor = _unitOfWork.ContractorsRepository.GetByID(id);
                return _mapper.Map<ContractorsDTO>(contractor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsChangedStatusContractorById(long id)
        {
            try
            {
                var contractor = _unitOfWork.ContractorsRepository.GetByID(id);
                if(contractor == null)
                {
                    throw new Exception("Contractor not found");
                }
                var account = _unitOfWork.AccountsRepository.GetByID(contractor.AccountId);
                if(account == null)
                {
                    throw new Exception("Account not found");
                }
                if (account.Status == true)
                {
                    account.Status = false;
                }
                else if (account.Status == false)
                {
                    account.Status = true;
                }
                _unitOfWork.AccountsRepository.Update(account);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ContractorsDTO UpdateContractor(long id, ContractorsPostDTO contractorsPostDTO)
        {
            try
            {
                var contractor = _unitOfWork.ContractorsRepository.GetByID(id);
                if (contractor == null)
                {
                    throw new Exception("Contractor not found");
                }
                var account = _unitOfWork.AccountsRepository.GetByID(contractor.AccountId);
                if (account == null)
                {
                    throw new Exception("Account not found");
                }
                _mapper.Map(contractorsPostDTO, contractor);
                _unitOfWork.ContractorsRepository.Update(contractor);
                _unitOfWork.Save();
                return _mapper.Map<ContractorsDTO>(contractor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
