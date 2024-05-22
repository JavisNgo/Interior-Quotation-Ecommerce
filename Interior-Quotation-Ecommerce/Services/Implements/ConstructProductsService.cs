using AutoMapper;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Repository.Implements;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class ConstructProductsService : IConstructProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConstructProductsService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }
        public bool IsUpdateQuantity(ConstructProductsPostDTO constructProductsPostDTO)
        {
            try
            {
                bool status = false;
                var construct = _unitOfWork.ConstructsRepository.GetByID(constructProductsPostDTO.ConstructId);
                var product = _unitOfWork.ProductsRepository.GetByID(constructProductsPostDTO.ProductId);
                var constructProduct = _unitOfWork.ConstructProductsRepository
                    .Get(cp => cp.ProductId == constructProductsPostDTO.ProductId && cp.ConstructId == constructProductsPostDTO.ConstructId)
                    .FirstOrDefault();
                if (construct != null && product != null && constructProduct != null)
                {
                    _mapper.Map(constructProduct, constructProductsPostDTO);
                    _unitOfWork.ConstructProductsRepository.Update(constructProduct);
                    _unitOfWork.Save();
                    status = true;
                }
                return status;
            }
            catch 
            {
                throw new Exception("Error at update quantity");
            }
        }
    }
}
