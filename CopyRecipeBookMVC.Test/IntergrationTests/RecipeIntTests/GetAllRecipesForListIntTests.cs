using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	[Collection("QueryCollection")]
	public class GetAllRecipesForListIntTests 		
	{
		private readonly RecipeService _recipeService;
		private readonly RecipeRepository _recipeRepo;

		private readonly IMapper _mapper;

        public GetAllRecipesForListIntTests(QueryTestFixtures fixtures)
        {
			var _context = fixtures.Context;
			var MapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			_mapper = MapperConfig.CreateMapper();

			_recipeRepo = new RecipeRepository(_context);
			_recipeService = new RecipeService(_recipeRepo, _mapper, null);
		}
		[Fact]
		public void NullSearchString_GetAllRecipesForList_ReturnCorrectList()
		{
			//Arrange
			var pageSize = 2;
			var pageNumber = 1;
			var searchString = "";
			//Act
			var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);	
			//Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);			
		}
		[Fact]
		public void SearchString_GetAllRecipesForList_ReturnCorrectList()
		{
			//Arrange
			var pageSize = 2;
			var pageNumber = 1;
			var searchString = "Test1";
			//Act
			var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Count);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);
		}
		[Fact]
		public void NotExistingSearchString_GetAllRecipesForList_ReturnEmptyList()
		{
			//Arrange
			var pageSize = 2;
			var pageNumber = 1;
			var searchString = "Test1Blabla";//notExistinginBase
			//Act
			var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			//Assert			
			Assert.Equal(0, result.Count);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);
		}

	}
}
