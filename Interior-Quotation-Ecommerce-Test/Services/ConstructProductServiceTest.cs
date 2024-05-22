using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Implements;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interior_Quotation_Ecommerce_Test.Services
{
    public class ConstructProductServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IConstructProductsService _service;
        private IGenericRepository<ConstructProducts> _fakeCpRepository;
        private IGenericRepository<Constructs> _fakeConstructsRepository;
        private IGenericRepository<Products> _fakeProductsRepository;

        public ConstructProductServiceTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _service = A.Fake<IConstructProductsService>();
            _fakeCpRepository = A.Fake<IGenericRepository<ConstructProducts>>();
            _fakeConstructsRepository = A.Fake<IGenericRepository<Constructs>>();
            _fakeProductsRepository = A.Fake<IGenericRepository<Products>>();
            _fakeCpRepository = A.Fake<IGenericRepository<ConstructProducts>>();

            A.CallTo(() => _unitOfWork.ConstructProductsRepository).Returns(_fakeCpRepository);
            A.CallTo(() => _unitOfWork.ConstructsRepository).Returns(_fakeConstructsRepository);
            A.CallTo(() => _unitOfWork.ProductsRepository).Returns(_fakeProductsRepository);
        }

        [Fact]
        public void ConstructProductService_IsUpdateQuantity_ReturnTrue()
        {
            var constructProductsPostDTO = new ConstructProductsPostDTO
            {
                ConstructId = 1,
                ProductId = 2,
                Quantity = 20
            };

            var construct = new Constructs { Id = 1 };
            var product = new Products { Id = 2 };
            var constructProduct = new ConstructProducts { ConstructId = 1, ProductId = 2, Quantity = 10 };

            _service = new ConstructProductsService(_unitOfWork, _mapper);

            A.CallTo(() => _fakeProductsRepository.GetByID(constructProductsPostDTO.ProductId)).Returns(product);
            A.CallTo(() => _fakeConstructsRepository.GetByID(constructProductsPostDTO.ConstructId)).Returns(construct);
            A.CallTo(() => _mapper.Map(constructProduct, constructProductsPostDTO)).Returns(constructProductsPostDTO);
            A.CallTo(() => _unitOfWork.ConstructProductsRepository.Update(constructProduct));
            A.CallTo(() => _fakeCpRepository.
                Get(A<Expression<Func<ConstructProducts, bool>>>.Ignored, A<Func<IQueryable<ConstructProducts>, IOrderedQueryable<ConstructProducts>>>.Ignored, "")).
                Returns(new List<ConstructProducts> { constructProduct });

            var result = _service.IsUpdateQuantity(constructProductsPostDTO);

            Assert.True(result);
        }

        [Fact]
        public void ConstructProductService_IsUpdateQuantity_ReturnFalse()
        {
            var constructProductsPostDTO = new ConstructProductsPostDTO
            {
                ConstructId = 1,
                ProductId = 2,
                Quantity = 20
            };

            var construct = new Constructs { Id = 1 };
            var product = new Products { Id = 1 };
            var constructProduct = new ConstructProducts { ConstructId = 1, ProductId = 1, Quantity = 10 };

            _service = new ConstructProductsService(_unitOfWork, _mapper);

            A.CallTo(() => _fakeProductsRepository.GetByID(constructProductsPostDTO.ProductId)).Returns(product);
            A.CallTo(() => _fakeConstructsRepository.GetByID(constructProductsPostDTO.ConstructId)).Returns(construct);
            A.CallTo(() => _mapper.Map(constructProduct, constructProductsPostDTO)).Returns(constructProductsPostDTO);
            A.CallTo(() => _unitOfWork.ConstructProductsRepository.Update(constructProduct));
            A.CallTo(() => _fakeCpRepository.
                Get(A<Expression<Func<ConstructProducts, bool>>>.Ignored, A<Func<IQueryable<ConstructProducts>, IOrderedQueryable<ConstructProducts>>>.Ignored, "")).
                Returns(new List<ConstructProducts> { });

            var result = _service.IsUpdateQuantity(constructProductsPostDTO);

            Assert.False(result);
        }
    }
}
