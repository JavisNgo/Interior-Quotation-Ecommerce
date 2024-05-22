using AutoMapper;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.MongoDB.Entities;
using Interior_Quotation_Ecommerce.MongoDB.Interfaces;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Interior_Quotation_Ecommerce.Utils;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class ConstructsService : IConstructsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConstructImagesRepository _constructImagesRepository;

        public ConstructsService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env, IConstructImagesRepository constructImagesRepository) 
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
            _constructImagesRepository = constructImagesRepository;
        }

        public List<ConstructsDTO> Get()
        {
            try
            {
                var constructsList = unitOfWork.ConstructsRepository?.Get(null, null, "Contractor");
                return _mapper.Map<List<ConstructsDTO>>(constructsList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while get construct, Message: " + ex.Message);
            }
        }

        public ConstructsDTO CreateConstruct(ConstructsPostDTO constructsPost, List<ConstructImagesPostDTO> fileUploads)
        {
            try
            {
                if(string.IsNullOrEmpty(constructsPost.Title))
                {
                    throw new ArgumentException("Construct's name is required", "400");
                }
                if(constructsPost.EstimatedPrice < 0)
                {
                    throw new ArgumentException("Estimated Price is smaller 0", "400");
                }
                var checkingContractor = unitOfWork.ContractorsRepository.GetByID(constructsPost.ContractorId);
                if (checkingContractor == null)
                {
                    throw new ArgumentException("Contractor not found", "404");
                }
                var checkingCategory = unitOfWork.CategoriesRepository.GetByID(constructsPost.CategoryId);
                if (checkingCategory == null)
                {
                    throw new ArgumentException("Category not found", "404");
                }
                string code = $"C_{constructsPost.ContractorId}_{MyUtils.GenerateRandomCode(10)}";
                bool checking = true;
                while (checking)
                {
                    if (unitOfWork.ConstructsRepository.Get(c => c.Code == code).FirstOrDefault() != null)
                    {
                        code = $"C_{constructsPost.ContractorId}_{MyUtils.GenerateRandomCode(10)}";
                    }
                    else
                    {
                        checking = false;
                    }
                };
                var constructs = _mapper.Map<Constructs>(constructsPost);
                constructsPost.Code = code;
                constructs.CreatedDate = DateTime.Now;
                unitOfWork.ConstructsRepository.Insert(constructs);
                unitOfWork.Save();

                var insertedConstruct = unitOfWork.ConstructsRepository.Get(c => c.Code == constructs.Code).FirstOrDefault();

                if (insertedConstruct == null) throw new Exception("Error while adding construct");

                if(insertedConstruct != null && constructsPost.constructProductsPost != null)
                {
                    if (constructsPost.constructProductsPost.Any())
                    {
                        foreach (var product in constructsPost.constructProductsPost)
                        {
                            var existedProduct = unitOfWork.ProductsRepository.GetByID(product.ProductId);
                            if (existedProduct != null)
                            {
                                var constructProduct = _mapper.Map<ConstructProducts>(product);
                                constructProduct.ConstructId = insertedConstruct.Id;
                                unitOfWork.ConstructProductsRepository.Insert(constructProduct);
                                unitOfWork.Save();
                            }
                        }
                    }
                }

                if(insertedConstruct != null && fileUploads.Count > 0)
                {
                    foreach (var file in fileUploads)
                    {
                        if (file.File.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.File.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                var constructImages = new ConstructImages
                                {
                                    ConstructId = file.ConstructId,
                                    Image = fileBytes
                                };
                                _constructImagesRepository.Create(constructImages);
                            }
                        }
                    }
                }

                return _mapper.Map<ConstructsDTO>(insertedConstruct);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, ex.ParamName);
            }
        }

        public bool IsDeleteConstruct(long deleteConstructId)
        {
            try
            {
                bool status = false;
                var constructProducts = unitOfWork.ConstructProductsRepository.Get(c => c.ConstructId == deleteConstructId).ToList();
                if (constructProducts.Any())
                {
                    foreach (var cp in constructProducts)
                    {
                        unitOfWork.ConstructProductsRepository.Delete(cp.Id);
                        unitOfWork.Save();
                    }
                }

                unitOfWork.ConstructsRepository.Delete(deleteConstructId);
                unitOfWork.Save();
                status = true;

                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ConstructsDTO UpdateConstruct(ConstructsPostDTO constructsPost, List<ConstructImagesPostDTO> fileUploads)
        {
            if (string.IsNullOrEmpty(constructsPost.Title))
            {
                throw new Exception("Construct's Title is required");
            }
            if (constructsPost.EstimatedPrice < 0)
            {
                throw new Exception("Estimated Price is smaller 0");
            }
            var checkingContractor = unitOfWork.ContractorsRepository.GetByID(constructsPost.ContractorId);
            if (checkingContractor == null)
            {
                throw new Exception("Contractor not found");
            }
            var checkingCategory = unitOfWork.CategoriesRepository.GetByID(constructsPost.CategoryId);
            if (checkingCategory == null)
            {
                throw new Exception("Category not found");
            }
            var constructEntity = unitOfWork.ConstructsRepository.Get(c => c.Code == constructsPost.Code, null, "").FirstOrDefault();
             constructEntity.UpdatedDate = DateTime.Now;

            if (fileUploads.Count > 0)
            {
                foreach (var file in fileUploads)
                {
                    if (file.File.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.File.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            var constructImages = new ConstructImages
                            {
                                ConstructId = file.ConstructId,
                                Image = fileBytes
                            };
                            _constructImagesRepository.Update(constructImages);
                        }
                    }
                }
            }

            _mapper.Map(constructsPost, constructEntity);
            unitOfWork.ConstructsRepository.Update(constructEntity);
            unitOfWork.Save();

            int countUrl = 0;
            
            return _mapper.Map<ConstructsDTO>(constructEntity);
        }

        public ConstructsDTO GetConstructById(long id)
        {
            try
            {
                var construct = unitOfWork.ConstructsRepository.GetByID(id);
                return _mapper.Map<ConstructsDTO>(construct);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
