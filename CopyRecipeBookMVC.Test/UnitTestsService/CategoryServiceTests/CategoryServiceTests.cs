using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Category;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsService.CategoryServiceTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoryService _categoryService;
        public CategoryServiceTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _categoryService = new CategoryService(_categoryRepoMock.Object, _mapperMock.Object);
        }
        [Fact]
        public void Get_GetListCategoryForList_ShouldGetAllList()
        {
            //Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name="Test1" },
                new Category { Id = 2,  Name="Test2"}
            };
            var categoryVms = new List<CategoryForListVm>
            {
                new CategoryForListVm { Id = 1, Name="Test1" },
                new CategoryForListVm { Id = 2, Name="Test2" }
            };
            _categoryRepoMock.Setup(repo => repo.GetAllCategories()).Returns(categories);
            _mapperMock.Setup(map => map.Map<List<CategoryForListVm>>(categories)).Returns(categoryVms);
            //Act
            var result = _categoryService.GetListCategoryForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListCategoryForListVm>(result);
            Assert.Equal(2, result.Categories.Count);
            Assert.Contains(result.Categories, c => c.Name == "Test1");
            Assert.Contains(result.Categories, c => c.Name == "Test2");
			Assert.Equal("Test1", result.Categories[0].Name); 
			Assert.Equal("Test2", result.Categories[1].Name);
		}        
		[Fact]
		public void GetListCategoryForList_NullFromRepository_ShouldReturnEmptyList()
		{
			// Arrange
			_categoryRepoMock.Setup(repo => repo.GetAllCategories()).Returns((List<Category>)null);
			_mapperMock.Setup(map => map.Map<List<CategoryForListVm>>(It.IsAny<List<Category>>())).Returns(new List<CategoryForListVm>());

			// Act
			var result = _categoryService.GetListCategoryForList();

			// Assert
			Assert.NotNull(result);
			Assert.IsType<ListCategoryForListVm>(result);
			Assert.Empty(result.Categories);
		}
		[Fact]
		public void GetListCategoryForList_NoCategories_ShouldReturnEmptyList()
		{
			// Arrange
			_categoryRepoMock.Setup(repo => repo.GetAllCategories()).Returns(new List<Category>());
			_mapperMock.Setup(map => map.Map<List<CategoryForListVm>>(It.IsAny<List<Category>>())).Returns(new List<CategoryForListVm>());

			// Act
			var result = _categoryService.GetListCategoryForList();

			// Assert
			Assert.NotNull(result);
			Assert.IsType<ListCategoryForListVm>(result);
			Assert.Empty(result.Categories);
		}

	}
}
