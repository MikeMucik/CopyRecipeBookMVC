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

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	[Collection("QueryCollection")]
	public class GetRecipesByIngredientsIntTests(QueryTestFixtures fixtures) : RecipeIntegrationView(fixtures)
	{
		[Fact]
		public void ReturnByIngredients_GetRecipesByIngredients_ReturnList()
		{
			//Arrange
			int pageSize = 2;
			int pageNumber = 1;
			List<int> ingredientIds = new List<int> {  1 };
			//Act
			var result = _recipeService.GetRecipesByIngredients(pageSize, pageNumber, ingredientIds, null);
			//Assert
			Assert.Equal(1, result.Count);
			Assert.NotNull(result);
			Assert.IsType<ListRecipesByIngredientsVm>(result);
		}
	}
}
