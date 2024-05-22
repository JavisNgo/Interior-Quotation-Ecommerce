using AutoMapper;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.MongoDB.Entities;
using Interior_Quotation_Ecommerce.MongoDB.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class ConstructImagesService : IConstructImagesService
    {
        private readonly IConstructImagesRepository _repository;
        private readonly IMapper _mapper;

        public ConstructImagesService(IConstructImagesRepository repository, IMapper mapper, IConstructsService constructsService) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ConstructImages CreateConstructImages(ConstructImagesPostDTO fileUpload)
        {
            try
            {
                if (fileUpload.File.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        fileUpload.File.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var constructImages = new ConstructImages
                        {
                            ConstructId = fileUpload.ConstructId,
                            Image = fileBytes
                        };
                        _repository.Create(constructImages);
                    }
                }
                return _mapper.Map<ConstructImages>(fileUpload);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, "500");
            }
        }

        public List<ConstructImages> GetConstructImages()
        {
            try
            {
                return _repository.GetAll();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ConstructImages> GetConstructImagesByConstructId(long ConstructId)
        {
            try
            {
                return _repository.GetAll().FindAll(c => c.ConstructId == ConstructId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsDeleteImageByConstructId(long ConstructId)
        {
            try
            {
                List<ConstructImages> list = _repository.GetAll().FindAll(c => c.ConstructId == ConstructId);
                if(list.Count > 0)
                {
                    foreach(var constructImage in list)
                    {
                        _repository.Delete(constructImage);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ConstructImages UpdateConstructImages(ConstructImagesPostDTO fileUpload)
        {
            try
            {
                if (fileUpload.File.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        fileUpload.File.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var constructImages = new ConstructImages
                        {
                            ConstructId = fileUpload.ConstructId,
                            Image = fileBytes
                        };
                        _repository.Update(constructImages);
                    }
                }
                return _mapper.Map<ConstructImages>(fileUpload);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, "500");
            }
        }

    }
}
