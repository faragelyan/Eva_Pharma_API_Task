using AutoMapper;
using BookShob.API.Controllers.Common;
using BookShob.Application.Interfaces;
using BookShob.Domain.DTOs;
using BookShob.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShob.Tests
{
    public class CategoryControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsListOfCategoryDtos()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCategoryRepo = new Mock<ICategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            var categories = new List<Category>
            {
                new Category { Id = 1, catName = "Science" },
                new Category { Id = 2, catName = "History" }
            };

            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, catName = "Science" },
                new CategoryDto { Id = 2, catName = "History" }
            };

            mockCategoryRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            mockUnitOfWork.Setup(u => u.CategoryRepository).Returns(mockCategoryRepo.Object);
            mockMapper.Setup(m => m.Map<List<CategoryDto>>(categories)).Returns(categoryDtos);

            var controller = new CategoryController(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsType<List<CategoryDto>>(okResult.Value);
            Assert.Equal(2, returnedDtos.Count);
        }
    }
}
