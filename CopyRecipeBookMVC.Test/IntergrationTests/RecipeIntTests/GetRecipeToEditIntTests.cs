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
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using SQLitePCL;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	[Collection("QueryCollection")]
	public class GetRecipeToEditIntTests(QueryTestFixtures fixtures) :  RecipeIntegrationView(fixtures)
	{
		[Fact]
		public void FindByExistingId_GetRecipeToEdit_ReturnFilledNewRecipeVm()
		{
			//Arrange
			var existingRecipe = 1;
			var baseRecipe = _context.Recipes.FirstOrDefault(x => x.Id == existingRecipe);
			//Act
			var result = _recipeService.GetRecipeToEdit(existingRecipe);
			//Assert
			Assert.NotNull(result);
			Assert.IsType<NewRecipeVm>(result);
			Assert.Equal(baseRecipe.Name, result.Name);
			Assert.Equal(1, result.CategoryId);
			Assert.Equal(1, result.DifficultyId);
			Assert.Equal(1, result.TimeId);
			Assert.Equal(baseRecipe.CategoryId, result.CategoryId);
			Assert.Equal(baseRecipe.DifficultyId, result.DifficultyId);
			Assert.Equal(baseRecipe.TimeId, result.TimeId);

			Assert.Equal(baseRecipe.RecipeIngredient.Count, result.Ingredients.Count);
			for (int i = 0; i < baseRecipe.RecipeIngredient.Count; i++)
			{
				var expectedIngredient = baseRecipe.RecipeIngredient.ElementAt(i);
				var actualIngredient = result.Ingredients[i];

				Assert.Equal(expectedIngredient.Ingredient.Id, actualIngredient.IngredientName);
				Assert.Equal(expectedIngredient.Unit.Id, actualIngredient.IngredientUnit);
				Assert.Equal(expectedIngredient.Quantity, actualIngredient.Quantity);
			}

		}
		[Fact]
		public void PutNotExistingId_GetRecipeToEdit_ReturnEmptyNewRecipeVm()
		{
			//Arrange
			var existingRecipe = -1;
			//Act
			var result = _recipeService.GetRecipeToEdit(existingRecipe);
			//Assert
			Assert.NotNull(result);
			Assert.IsType<NewRecipeVm>(result);
			Assert.Equal(null, result.Name);
			Assert.Equal(null, result.Description);
			Assert.Equal(0, result.CategoryId);
			Assert.Equal(0, result.DifficultyId);
			//Assert.Equal(0, result.TimeId);
			Assert.Equal([], result.Ingredients);
			Assert.Null(result.TimeId); //   NewRecipeVm int? TimeId
		}
	}
}
