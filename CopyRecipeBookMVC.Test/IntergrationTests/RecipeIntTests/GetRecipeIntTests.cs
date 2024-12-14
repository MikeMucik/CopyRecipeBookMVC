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
	public class GetRecipeIntTests(QueryTestFixtures fixtures) : RecipeIntegrationView(fixtures)
	{
		[Fact]
		public void FindByExistingId_GetRecipe_ReturnRecipeFullInfo()
		{
			//Arrange
			var existingIdOfRecipe = 1;
			//Act
			var result = _recipeService.GetRecipe(existingIdOfRecipe);
			//Assert
			var baseRecipe = _context. Recipes.FirstOrDefault(x => x.Id == existingIdOfRecipe);			
			Assert.Equal(baseRecipe.Id, result.Id);
			Assert.Equal(baseRecipe.Name, result.Name);
			Assert.Equal(baseRecipe.Category.Name, result.Category);			
			Assert.Equal(baseRecipe.Difficulty.Name, result.Difficulty);
			Assert.Equal(baseRecipe.Time.Amount + " " + baseRecipe.Time.Unit, result.Time);
			Assert.Equal(baseRecipe.Description, result.Description);

			Assert.Equal(baseRecipe.RecipeIngredient.Count, result.Ingredients.Count);
			for (int i = 0; i < baseRecipe.RecipeIngredient.Count; i++)
			{
				var expectedIngredient = baseRecipe.RecipeIngredient.ElementAt(i);
				var actualIngredient = result.Ingredients[i];

				Assert.Equal(expectedIngredient.Ingredient.Name, actualIngredient.Name);
				Assert.Equal(expectedIngredient.Unit.Name, actualIngredient.Unit);
				Assert.Equal(expectedIngredient.Quantity, actualIngredient.Quantity);				
			}
		}
		[Fact]
		public void NotExistingId_GetRecipe_ReturnException()
		{
			//Arrange
			var existingIdOfRecipe = -1;
			//Act			
			void result()=> _recipeService.GetRecipe(existingIdOfRecipe);
			//Assert
			var exception = Assert.Throws<InvalidOperationException>(result);
			Assert.Equal($"Przepis o Id '{existingIdOfRecipe}' nie istnieje.", exception.Message);			
		}

	}
}
