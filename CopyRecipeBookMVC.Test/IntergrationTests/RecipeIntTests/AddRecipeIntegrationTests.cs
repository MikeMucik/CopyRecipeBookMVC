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
	public class AddRecipeIntegrationTests : CommandTestBase

	{
		private readonly RecipeService _recipeService;
		private readonly RecipeRepository _recipeRepo;
		private readonly IRecipeIngredientService _recipeIngredientService;
		private readonly IMapper _mapper;

		private readonly IIngredientService _ingredientService;		
		private readonly IUnitService _unitService;		
		public AddRecipeIntegrationTests() : base()
		{	
			var MapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();				
			});
			_mapper = MapperConfig.CreateMapper();

			_recipeRepo = new RecipeRepository(_context);
			var _ingredientRepo = new IngredientRepository(_context);
			var _unitRepo = new UnitRepository(_context);
			var _recipeIngredientRepo = new RecipeIngredientRepository(_context);

			_ingredientService = new IngredientService(_ingredientRepo, _mapper);
			_unitService = new UnitService(_unitRepo, _mapper);			
			_recipeIngredientService = new RecipeIngredientService(_recipeIngredientRepo, _mapper);
			_recipeService = new RecipeService(_recipeRepo, _mapper, _recipeIngredientService);
		}
		[Fact]
		public void AddProperData_AddRecipe_ShouldAddToColletion()
		{
			//Arrange
			var recipe = new NewRecipeVm
			{
				Id = 3,
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
			var addedRecipe = _context.Recipes.Find(3);
			Assert.NotNull(addedRecipe);
			Assert.Equal("Test3", addedRecipe.Name);
			Assert.Equal(3, result);

			var addedIngredients = _context.RecipeIngredient.Where(x => x.RecipeId == 3).ToList();
			Assert.Equal(2, addedIngredients.Count);
			Assert.Contains(addedIngredients, i => i.IngredientId == 1 && i.Quantity == 1);
			Assert.Contains(addedIngredients, i => i.IngredientId == 2 && i.Quantity == 2);
		}

		[Fact]
		public void AddNewData_AddRecipe_ShouldAddToColletion()
		{
			//Arrange
			var recipe = new NewRecipeVm
			{
				Id = 4,
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
			var addedRecipe = _context.Recipes.Find(4);
			Assert.NotNull(addedRecipe);
			Assert.Equal("Test4", addedRecipe.Name);
			Assert.Equal(4, result);

			var addedIngredients = _context.RecipeIngredient.Where(x => x.RecipeId == 4).ToList();
			Assert.Equal(2, addedIngredients.Count);
			
			Assert.Contains(addedIngredients, i => i.IngredientId == 3 && i.Unit.Id == 3);
			Assert.Contains(addedIngredients, i => i.IngredientId == 4 && i.Unit.Id == 4);
		}
		[Fact]
		public void AddInvalidData_AddRecipe_ShouldNotAddToColletion()
		{
			//Arrange
			var recipe = new NewRecipeVm
			{
				Id = 1,
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
			//var result = _recipeService.AddRecipe(recipe);
			var exception = Assert.Throws<InvalidOperationException>(() => _recipeService.AddRecipe(recipe));
			//Assert			
			Assert.Equal("Przepis o Id '1' już istnieje.", exception.Message);
		}
	}
}
