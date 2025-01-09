using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	[Collection("QueryCollection")]
	public class GetRecipesByCategoryIntTests(QueryTestFixtures fixtures) : RecipeIntegrationView(fixtures)
	{
		[Fact]
        public void ReturnByCategory_GetRecipesByCategory_ReturnListRecipesByCategory()
        {
            //Arrange
            var category = 1;
            var pageNumber = 1;
            var pageSize = 2;
			var categoryName = "";
            //Act
            var result = _recipeService.GetRecipesByCategory(pageSize, pageNumber, category, categoryName);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.IsType<ListRecipesByCategoryVm>(result);
        }
		[Fact]
		public void ReturnByNotExistingCategory_GetRecipesByCategory_ReturnException()
		{
			//Arrange
			var category = 0;
			var pageNumber = 1;
			var pageSize = 2;
			var categoryName = "";
			//Act
			var exception =Assert.Throws<ArgumentException>(()=>
			_recipeService.GetRecipesByCategory(pageSize, pageNumber, category, categoryName));
			//var result = _recipeService.GetRecipesByCategory(pageSize, pageNumber, category, categoryName);
			//Assert
			Assert.Equal("Kategoria musi być określona przez ID lub nazwę.", exception.Message);			
			
		}

	}
}
