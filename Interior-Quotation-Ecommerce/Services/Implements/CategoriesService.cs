using AutoMapper;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CategoriesDTO AddCategory(CategoriesDTO categoryView)
        {
            try
            {
                var category = _mapper.Map<Categories>(categoryView);
                category.CreatedDate = DateTime.Now;
                unitOfWork.CategoriesRepository.Insert(category);
                unitOfWork.Save();
                return categoryView;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the category. Error message: {ex.Message}");
            }
        }

        public CategoriesDTO DeleteCategory(int id)
        {
            try
            {
                var category = unitOfWork.CategoriesRepository?.GetByID(id);

                if (category == null)
                {
                    return null;
                }

                var existingConstruct = unitOfWork.ConstructsRepository?.Get(c => c.CategoryId == category.Id).ToList();
                if (existingConstruct.Any())
                {
                    throw new Exception("Please delete or change category of constructs that contain this category before deleting.");
                }


                unitOfWork.CategoriesRepository?.Delete(id);
                unitOfWork.Save();

                return _mapper.Map<CategoriesDTO>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category. Error message: {ex.Message}");
            }
        }

        public IEnumerable<CategoriesDTO> GetCategories()
        {
            try
            {
                var categoriesEntities = unitOfWork.CategoriesRepository?.Get();
                var categoriesDTOs = _mapper.Map<IEnumerable<CategoriesDTO>>(categoriesEntities);
                return categoriesDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while get the category. Error message: " + ex.Message);
            }
        }

        public CategoriesDTO GetCategoryById(int id)
        {
            try
            {
                var categoryEntity = unitOfWork.CategoriesRepository.GetByID(id);
                return _mapper.Map<CategoriesDTO>(categoryEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while get the category. Error message: " + ex.Message);
            }
        }

        public CategoriesDTO UpdateCategory(int id, CategoriesDTO updatedCategoryView)
        {
            try
            {
                var existingCategory = unitOfWork.CategoriesRepository?.GetByID(id);

                if (existingCategory == null)
                {
                    return null;
                }

                _mapper.Map(updatedCategoryView, existingCategory);
                existingCategory.UpdatedDate = DateTime.Now;
                unitOfWork.CategoriesRepository?.Update(existingCategory);
                unitOfWork.Save();

                return updatedCategoryView;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the category. Error message: {ex.Message}");
            }
        }
    }
}
