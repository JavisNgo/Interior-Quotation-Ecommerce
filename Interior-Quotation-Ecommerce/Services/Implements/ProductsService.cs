using AutoMapper;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Interior_Quotation_Ecommerce.Utils;
using System.Text;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _imagesDirectory;

        public ProductsService(IUnitOfWork unitOfWork,IMapper mapper, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagesDirectory = Path.Combine(env.ContentRootPath, "img", "productImage");
        }
        public ProductsDTO AddProduct(ProductsPostDTO productsPostDTO)
        {
            try
            {
                string code = $"P_{productsPostDTO.ContractorId}_{MyUtils.GenerateRandomCode(10)}";
                bool checking = true;
                while (checking)
                {
                    if (unitOfWork.ProductsRepository.Get(p => p.Code == code).FirstOrDefault() != null)
                    {
                        code = $"P_{productsPostDTO.ContractorId}_{MyUtils.GenerateRandomCode(10)}";
                    }
                    else
                    {
                        checking = false;
                    }
                };
                var product = _mapper.Map<Products>(productsPostDTO);
                product.Code = code;

                unitOfWork.ProductsRepository.Insert(product);
                unitOfWork.Save();

                var createdProduct = unitOfWork.ProductsRepository.Get(p => p.Code == code).FirstOrDefault();

                return _mapper.Map<ProductsDTO>(createdProduct);
            }
            catch (ArgumentException ex)
            {
                throw;
            }
        }

        public IEnumerable<ProductsDTO> GetAll()
        {
            var products = unitOfWork.ProductsRepository.Get(null, null, "ProductImages");
            return _mapper.Map<IEnumerable<ProductsDTO>>(products);
        }

        public ProductsDTO GetById(long id)
        {
            var product = unitOfWork.ProductsRepository.Get(p => p.Id == id, null, "ProductImages").FirstOrDefault();
            if(product == null)
            {
                return null;
            }
            return _mapper.Map<ProductsDTO>(product);
        }

        public bool IsDeleteProduct(long id)
        {
            throw new NotImplementedException();
        }

        public ProductsDTO UpdateProduct(ProductsPostDTO productsPostDTO)
        {
            try
            {
                var existingProduct = unitOfWork.ProductsRepository.Get(p => p.Code == productsPostDTO.Code).FirstOrDefault();

                if (existingProduct == null)
                {
                    return null;
                }

                _mapper.Map(productsPostDTO, existingProduct);
                unitOfWork.ProductsRepository.Update(existingProduct);
                unitOfWork.Save();

                return _mapper.Map<ProductsDTO>(existingProduct);
            } catch (Exception ex)
            {
                throw new Exception("Error at update product: " + ex.Message);
            }
        }
    }
}
