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
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	public class UpdateRecipeIntTests :RecipeIntegrationCommand
	{
		[Fact]
		public void EnterCorrectDataExistingIngredient_UpdateRecipe_SaveNewDataToRecipe()
		{
			// Arrange
			using var arrangeContext = new Context(_contextOptions);
			arrangeContext.Recipes.Add(new Recipe
			{
				Id = 10,
				Name = "Original Recipe",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Original Description"
			});

			arrangeContext.Ingredients.Add(new Ingredient { Id = 3, Name = "Salt" });
			arrangeContext.Units.Add(new Unit { Id = 3, Name = "Gram" });
			arrangeContext.RecipeIngredient.Add(new RecipeIngredient { RecipeId = 10, IngredientId = 3, UnitId = 3, Quantity = 1 });
			arrangeContext.SaveChanges();

			//Act
			using (var actContext = new Context(_contextOptions))
			{
				var recipeRepo = new RecipeRepository(actContext);
				var recipeIngredientRepo = new RecipeIngredientRepository(actContext);
				var recipeIngredientService = new RecipeIngredientService(recipeIngredientRepo, _mapper);
				var recipeService = new RecipeService(recipeRepo, _mapper, recipeIngredientService);

				var newIngredient = new IngredientForNewRecipeVm
				{
					IngredientName = 3, // Id istniejącego składnika
					IngredientUnit = 3, // Id istniejącej jednostki
					Quantity = 100
				};
				var newRecipe = new NewRecipeVm
				{
					Id = 10, // Przepis, który istnieje w bazie
					Name = "Updated Recipe",
					CategoryId = 2,
					DifficultyId = 2,
					TimeId = 3,
					Ingredients = new List<IngredientForNewRecipeVm> { newIngredient },
					Description = "Updated Recipe Description"
				};
				recipeService.UpdateRecipe(newRecipe);
				actContext.SaveChanges();
			}

			// Assert
			using (var assertContext = new Context(_contextOptions))
			{
				var result = assertContext.Recipes
					.Include(r => r.RecipeIngredient)
					.ThenInclude(ri => ri.Ingredient)
					.Include(r => r.RecipeIngredient)
					.ThenInclude(ri => ri.Unit)
					.FirstOrDefault(r => r.Id == 10);

				Assert.NotNull(result);
				Assert.Equal("Updated Recipe", result.Name);
				Assert.Equal(2, result.CategoryId);
				Assert.Equal(2, result.DifficultyId);
				Assert.Equal(3, result.TimeId);
				Assert.Equal("Updated Recipe Description", result.Description);

				Assert.Single(result.RecipeIngredient);
				Assert.Equal(3, result.RecipeIngredient.First().IngredientId);
				Assert.Equal(3, result.RecipeIngredient.First().UnitId);
				Assert.Equal(100, result.RecipeIngredient.First().Quantity);
			}
		}
		[Fact]
		public void EnterCorrectDataNewIngredient_UpdateRecipe_SaveNewDataToRecipe()
		{
			// Arrange
			using var arrangeContext = new Context(_contextOptions);
			arrangeContext.Recipes.Add(new Recipe
			{
				Id = 100,
				Name = "Original Recipe",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Original Description"
			});
			arrangeContext.Ingredients.Add(new Ingredient { Id = 20, Name = "Salt" });
			arrangeContext.Units.Add(new Unit { Id = 20, Name = "Gram" });
			arrangeContext.RecipeIngredient.Add(new RecipeIngredient { RecipeId = 100, IngredientId = 20, UnitId = 20, Quantity = 1 });
			arrangeContext.SaveChanges();
			//Act
			using (var actContext = new Context(_contextOptions))
			{
				var recipeRepo = new RecipeRepository(actContext);
				var recipeIngredientRepo = new RecipeIngredientRepository(actContext);
				var recipeIngredientService = new RecipeIngredientService(recipeIngredientRepo, _mapper);
				var recipeService = new RecipeService(recipeRepo, _mapper, recipeIngredientService);
				var ingredientRepo = new IngredientRepository(actContext);
				var ingredientService = new IngredientService(ingredientRepo, _mapper);
				var unitRepo = new UnitRepository(actContext);
				var unitService = new UnitService(unitRepo, _mapper);
				var oldIngredient = new IngredientForNewRecipeVm
				{
					IngredientName = 20,
					IngredientUnit =20,
					Quantity = 1,
				};
				var newIngredient = new IngredientForNewRecipeVm
				{
					NewIngredientName = "Flour", // nazwa nieistniejącego składnika
					NewIngredientUnit = "Kilogram", // nazwa nieistniejącej jednostki
					Quantity = 100
				};
				var newRecipe = new NewRecipeVm
				{
					Id = 100, // Przepis, który istnieje w bazie
					Name = "Updated Recipe",
					CategoryId = 2,
					DifficultyId = 2,
					TimeId = 3,
					Ingredients = new List<IngredientForNewRecipeVm> { newIngredient, oldIngredient },
					Description = "Updated Recipe Description"
				};
				newIngredient.IngredientName = ingredientService.GetOrAddIngredient(newIngredient);
				actContext.SaveChanges();
				newIngredient.IngredientUnit = unitService.GetOrAddUnit(newIngredient);
				actContext.SaveChanges();
				recipeService.UpdateRecipe(newRecipe);
				actContext.SaveChanges();
			}
			// Assert
			using (var assertContext = new Context(_contextOptions))
			{
				var result = assertContext.Recipes
					.Include(r => r.RecipeIngredient)
					.ThenInclude(ri => ri.Ingredient)
					.Include(r => r.RecipeIngredient)
					.ThenInclude(ri => ri.Unit)
					.FirstOrDefault(r => r.Id == 100);

				Assert.NotNull(result);
				Assert.Equal("Updated Recipe", result.Name);
				Assert.Equal(2, result.CategoryId);
				Assert.Equal(2, result.DifficultyId);
				Assert.Equal(3, result.TimeId);
				Assert.Equal("Updated Recipe Description", result.Description);
				
				Assert.Equal(100, result.RecipeIngredient.First().Quantity);
				Assert.Equal(2, result.RecipeIngredient.Count());
				Assert.True(assertContext.Ingredients.Any(x => x.Name == "Flour"), "The ingredient 'Flour' was not found in the database.");
				Assert.True(assertContext.Units.Any(x => x.Name == "Kilogram"), "The ingredient 'Kilogram' was not found in the database.");				
			}
		}
		[Fact]
		public void EnterInCorrectDataNewIngredient_UpdateRecipe_NSaveNewDataToRecipeWithoutNewIngredient()
		{
			// Arrange
			using var arrangeContext = new Context(_contextOptions);
			arrangeContext.Recipes.Add(new Recipe
			{
				Id = 110,
				Name = "Original Recipe",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Original Description"
			});
			arrangeContext.Ingredients.Add(new Ingredient { Id = 120, Name = "Salt" });
			arrangeContext.Units.Add(new Unit { Id = 120, Name = "Gram" });
			arrangeContext.RecipeIngredient.Add(new RecipeIngredient { RecipeId = 110, IngredientId = 120, UnitId = 120, Quantity = 1 });
			arrangeContext.SaveChanges();
			//Act
			using (var actContext = new Context(_contextOptions))
			{
				var recipeRepo = new RecipeRepository(actContext);
				var recipeIngredientRepo = new RecipeIngredientRepository(actContext);
				var recipeIngredientService = new RecipeIngredientService(recipeIngredientRepo, _mapper);
				var recipeService = new RecipeService(recipeRepo, _mapper, recipeIngredientService);
				var ingredientRepo = new IngredientRepository(actContext);
				var ingredientService = new IngredientService(ingredientRepo, _mapper);
				var unitRepo = new UnitRepository(actContext);
				var unitService = new UnitService(unitRepo, _mapper);
				var oldIngredient = new IngredientForNewRecipeVm
				{
					IngredientName = 120,
					IngredientUnit = 120,
					Quantity = 1,
				};
				var newIngredient = new IngredientForNewRecipeVm
				{
					NewIngredientName = "Flour1", // nazwa nieistniejącego składnika
					//NewIngredientUnit = "Kilogram", // nazwa nieistniejącej jednostki
					Quantity = 100
				};
				var newRecipe = new NewRecipeVm
				{
					Id = 110, // Przepis, który istnieje w bazie
					Name = "Updated Recipe",
					CategoryId = 2,
					DifficultyId = 2,
					TimeId = 3,
					Ingredients = new List<IngredientForNewRecipeVm> { newIngredient, oldIngredient },
					Description = "Updated Recipe Description"
				};
				newIngredient.IngredientName = ingredientService.GetOrAddIngredient(newIngredient);
				actContext.SaveChanges();
				newIngredient.IngredientUnit = unitService.GetOrAddUnit(newIngredient);
				actContext.SaveChanges();
				recipeService.UpdateRecipe(newRecipe);
				actContext.SaveChanges();
			}
			// Assert
			using (var assertContext = new Context(_contextOptions))
			{
				var result = assertContext.Recipes
					.Include(r => r.RecipeIngredient)
					.ThenInclude(ri => ri.Ingredient)
					.Include(r => r.RecipeIngredient)
					.ThenInclude(ri => ri.Unit)
					.FirstOrDefault(r => r.Id == 110);

				Assert.NotNull(result);
				Assert.Equal("Updated Recipe", result.Name);
				Assert.Equal(2, result.CategoryId);
				Assert.Equal(2, result.DifficultyId);
				Assert.Equal(3, result.TimeId);
				Assert.Equal("Updated Recipe Description", result.Description);

				Assert.Equal(1, result.RecipeIngredient.First().Quantity);
				Assert.Equal(1, result.RecipeIngredient.Count());
				Assert.True(assertContext.Ingredients.Any(x => x.Name == "Flour1"), "The ingredient 'Flour' was not found in the database.");				
			}			
		}
		[Fact]
		public void RemoveIngredient_UpdateRecipe_IngredientRemovedFromRecipe()
		{

		}
		[Fact]
		 public void NonExistingIngredient_UpdateRecipe_RecipeUpdateFails()
		{

		}
		[Fact]
		public void DuplicateIngredients_UpdateRecipe_RecipeUpdateFails()
		{

		}
	}
}

