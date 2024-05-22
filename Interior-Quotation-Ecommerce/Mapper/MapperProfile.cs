using AutoMapper;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.MongoDB.Entities;

namespace Interior_Quotation_Ecommerce.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Accounts, AccountsDTO>().ReverseMap();
            CreateMap<Categories, CategoriesDTO>().ReverseMap();
            //ConstructImages Mapper
            //ConstructProducts Mapper
            CreateMap<ConstructProducts, ConstructProductsDTO>();
            CreateMap<ConstructProductsPostDTO, ConstructProducts>();
            //Connstruct Mapper
            CreateMap<Constructs, ConstructsDTO>();
            CreateMap<ConstructsPostDTO, Constructs>();
            //Contractor Mapper
            CreateMap<Contractors, ContractorsDTO>();
            CreateMap<ContractorsPostDTO, Contractors>();

            CreateMap<Customers, CustomerDTO>().ReverseMap();
            CreateMap<ProductImages, ProductImages>().ReverseMap();

            //FileUpload 
            CreateMap<ConstructImagesPostDTO, ConstructImages>().ReverseMap();
            CreateMap<ConstructImagesPostDTO, ProductImages>().ReverseMap();

            //Product Mapper
            CreateMap<Products, ProductsDTO>();
            CreateMap<ProductsPostDTO, Products>();

            CreateMap<RequestDetails, RequestDetailsDTO>().ReverseMap();
            CreateMap<Requests, RequestsDTO>().ReverseMap();

        }
    }
}
