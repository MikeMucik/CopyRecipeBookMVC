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

			var mockRepo = new Mock<ICategoryRepository>();
			mockRepo
				.Setup(repo => repo.GetAllCategories())
				.Returns(categories);

			var mockMapper = new Mock<IMapper>();
			mockMapper
				.Setup(map => map.Map<List<CategoryForListVm>>(categories))
				.Returns(categoryVms);

			var mockService = new CategoryService(mockRepo.Object, mockMapper.Object);
			//Act
			var result = mockService.GetListCategoryForList();
			//Assert
			Assert.NotNull(result);
			Assert.IsType<ListCategoryForListVm>(result);
			Assert.Equal(2, result.Categories.Count);
			Assert.Contains(result.Categories, c => c.Name == "Test1");
			Assert.Contains(result.Categories, c => c.Name == "Test2");
		}
	}
}
