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
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	public class AddRecipeIntegrationTests : RecipeIntegrationCommand		

	{		
		[Fact]
		public void AddProperData_AddRecipe_ShouldAddToColletion()
		{
			//Arrange
			var recipe = new NewRecipeVm
			{
				Name = "Test3",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Test3",
				Ingredients = new List<IngredientForNewRecipeVm>
				{
					new IngredientForNewRecipeVm {IngredientName = 1, IngredientUnit = 1,Quantity = 1,},
					new IngredientForNewRecipeVm {IngredientName = 2,IngredientUnit = 1,Quantity = 2,}
				},
				NumberOfIngredients = 2,
			};
			//Act			
			foreach (var item in recipe.Ingredients)
			{
				item.IngredientName = _ingredientService.GetOrAddIngredient(item);
				item.IngredientUnit = _unitService.GetOrAddUnit(item);
			}
			var result = _recipeService.AddRecipe(recipe);
			//Assert
			var addedRecipe = _context.Recipes.Find(result);
			Assert.NotNull(addedRecipe);
			Assert.Equal("Test3", addedRecipe.Name);
			//Assert.Equal(3, result);

			var addedIngredients = _context.RecipeIngredient.Where(x => x.RecipeId == result).ToList();
			Assert.Equal(2, addedIngredients.Count);
			Assert.Contains(addedIngredients, i => i.IngredientId == 1 && i.Quantity == 1);
			Assert.Contains(addedIngredients, i => i.IngredientId == 2 && i.Quantity == 2);
		}
		[Fact]
		public void AddNewIngredientData_AddRecipe_ShouldAddToColletion()
		{
			//Arrange
			var recipe = new NewRecipeVm
			{				
				Name = "Test4",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Test4",
				Ingredients = new List<IngredientForNewRecipeVm>
				{
					new IngredientForNewRecipeVm {NewIngredientName = "potato", NewIngredientUnit = "kg",Quantity = 1,},
					new IngredientForNewRecipeVm {NewIngredientName = "tomato", NewIngredientUnit = "can",Quantity = 2,}
				},
				NumberOfIngredients = 2,
			};
			//Act			
			foreach (var item in recipe.Ingredients)
			{
				item.IngredientName = _ingredientService.GetOrAddIngredient(item);
				item.IngredientUnit = _unitService.GetOrAddUnit(item);
			}
			var result = _recipeService.AddRecipe(recipe);
			//Assert
			var addedRecipe = _context.Recipes.Find(result);
			Assert.NotNull(addedRecipe);
			Assert.Equal("Test4", addedRecipe.Name);
			//Assert.Equal(4, result);

			var addedIngredients = _context.RecipeIngredient.Where(x => x.RecipeId == result).ToList();
			Assert.Equal(2, addedIngredients.Count);			
			Assert.True(_context .Ingredients.Any(x => x.Name == "potato"), "The ingredient 'Flour' was not found in the database.");
			Assert.True(_context .Ingredients.Any(x => x.Name == "tomato"), "The ingredient 'Flour' was not found in the database.");
			Assert.True(_context.Units.Any(x => x.Name == "kg"), "The ingredient 'Kilogram' was not found in the database.");
			Assert.True(_context.Units.Any(x => x.Name == "can"), "The ingredient 'Kilogram' was not found in the database.");
			
		}
		[Fact]
		public void AddInvalidDataTheSameName_AddRecipe_ShouldNotAddToColletion()
		{
			//Arrange
			var recipe = new NewRecipeVm
			{
				Name = "Test",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Test3",
				Ingredients = new List<IngredientForNewRecipeVm>
				{
					new IngredientForNewRecipeVm {IngredientName = 1, IngredientUnit = 1,Quantity = 1,},
					new IngredientForNewRecipeVm {IngredientName = 2, IngredientUnit = 1,Quantity = 2,}
				},
				NumberOfIngredients = 2,
			};
			//Act			
			foreach (var item in recipe.Ingredients)
			{
				item.IngredientName = _ingredientService.GetOrAddIngredient(item);
				item.IngredientUnit = _unitService.GetOrAddUnit(item);
			}			
			var exception = Assert.Throws<InvalidDataException>(() => _recipeService.AddRecipe(recipe));
			//Assert			
			Assert.Equal($"Przepis o Nazwie '{recipe.Name}' już istnieje.", exception.Message);
		}		
		[Fact]
		public void AddNullData_AddRecipe_ShouldNotAddToColletion()
		{
			//Arrange			
			//Act					
			var exception = Assert.Throws<ArgumentNullException>(() => _recipeService.AddRecipe(null));
			//Assert			
			Assert.Equal("Nieprawidłowe dane (Parameter 'recipe')", exception.Message);
		}
	}
}
