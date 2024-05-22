using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Implements;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interior_Quotation_Ecommerce_Test.Services
{
    public class ProductsServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IProductsService _service;

        public ProductsServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _env = A.Fake<IWebHostEnvironment>();
            _service = new ProductsService(_unitOfWork, _mapper, _env);
        }

        [Fact]
        public void ProductsService_GetAll_ReturnNotNull()
        {
            var products = A.Fake<IEnumerable<Products>>();
            var productsDTO = A.Fake<IEnumerable<ProductsDTO>>();
            A.CallTo(() => _mapper.Map<IEnumerable<ProductsDTO>>(products)).Returns(productsDTO);

            var result = _service.GetAll();

            Assert.NotNull(result);
        }

        [Fact]
        public void ProductsService_AddProduct_ReturnProductsDTO()
        {
            //Arrange
            var productPost = A.Fake<ProductsPostDTO>();
            productPost.Status = true;
            productPost.ContractorId = 1;
            productPost.Description = "Chair with chair";
            productPost.Code = $"P_1_1234567890";
            productPost.Name = "Chair Test";
            productPost.Price = 1000;

            var product = A.Fake<Products>();
            product.Id = 1;
            product.Status = true;
            product.ContractorId = 1;
            product.Description = "Chair with chair";
            product.Code = $"P_1_1234567890";
            product.Name = "Chair Test";
            product.Price = 1000;

            A.CallTo(() => _mapper.Map<Products>(productPost)).Returns(product);

            // Giả lập lần đầu trả về null
            A.CallTo(() => _unitOfWork.ProductsRepository.Get(A<Expression<Func<Products, bool>>>.That.Matches(exp =>
                exp.Compile()(product) == false), null, ""))
                .Returns(new List<Products>().AsQueryable());

            // Giả lập lần thứ hai trả về đối tượng giả lập
            A.CallTo(() => _unitOfWork.ProductsRepository.Get(A<Expression<Func<Products, bool>>>.That.Matches(exp =>
                exp.Compile()(product) == true), null, ""))
                .Returns(new List<Products> { product }.AsQueryable());

            //Act
            var result = _service.AddProduct(productPost);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
