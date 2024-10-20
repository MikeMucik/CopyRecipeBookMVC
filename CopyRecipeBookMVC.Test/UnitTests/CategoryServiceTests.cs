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

namespace CopyRecipeBookMVC.Test
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
		}
		//[Fact]
		//public void GetCategorySelectList_ShouldReturnCorrectList()
		//{
		//	var mockCategoryRepo = new Mock<ICategoryRepository>();
		//	var mockMapper = new Mock<IMapper>();

		//	// Dane z repozytorium (surowe dane kategorii)
		//	var categories = new List<Category>
		//	{
		//		new Category { Id = 1, Name = "Category 1" },
		//		new Category { Id = 2, Name = "Category 2" }
		//	};

		//	// Dane po mapowaniu przez AutoMapper (obiekty CategoryForListVm)
		//	var categoryVms = new List<CategoryForListVm>
		//	{
		//		new CategoryForListVm { Id = 1, Name = "Category 1" },
		//		new CategoryForListVm { Id = 2, Name = "Category 2" }
		//	};

		//	// Mockowanie repozytorium, które zwraca surowe dane kategorii
		//	mockCategoryRepo.Setup(repo => repo.GetAllCategories()).Returns(categories);

		//	// Mockowanie AutoMappera, który mapuje surowe dane na widoki kategorii
		//	mockMapper.Setup(m => m.Map<List<CategoryForListVm>>(categories)).Returns(categoryVms);

		//	// Tworzymy serwis z zamockowanym repozytorium i mapperem
		//	var categoryService = new CategoryService(mockCategoryRepo.Object, mockMapper.Object);

		//	// Act
		//	var result = categoryService.GetListCategoryForList();

		//	// Assert
		//	Assert.NotNull(result); // Sprawdzamy, czy wynik nie jest null
		//	Assert.Equal(2, result.Categories.Count); // Sprawdzamy, czy są dokładnie dwie kategorie
		//	Assert.Equal("Category 1", result.Categories[0].Name); // Sprawdzamy poprawność mapowania pierwszej kategorii
		//	Assert.Equal("Category 2", result.Categories[1].Name);
		//}
	}
}
