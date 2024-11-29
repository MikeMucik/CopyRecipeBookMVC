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
	public class GetRecipeIntTests  : CommandTestBase//bo musze użyć _context by porównać
	{
		private readonly RecipeService _recipeService;
		private readonly RecipeRepository _recipeRepo;
		private readonly IMapper _mapper;

		//private readonly Context _context;
		private readonly IRecipeIngredientService _recipeIngredientService;
		private readonly IIngredientService _ingredientService;
		private readonly IUnitService _unitService;

		public GetRecipeIntTests(QueryTestFixtures fixtures) 
		{
			var _context = fixtures.Context;
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
		public void FindByExistingId_GetRecipe_ReturnRecipeFullInfo()
		{
			//Arrange
			var existingIdOfRecipe = 1;
			//Act
			var result = _recipeService.GetRecipe(existingIdOfRecipe);
			//Assert
			var baseRecipe = _context. Recipes.FirstOrDefault(x => x.Id == existingIdOfRecipe);
			//var recipeOriginal = QueryTestFixtures.
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
			//var result = _recipeService.GetRecipe(existingIdOfRecipe);
			void result()=> _recipeService.GetRecipe(existingIdOfRecipe);
			//Assert
			var exception = Assert.Throws<InvalidOperationException>(result);
			Assert.Equal($"Przepis o Id '{existingIdOfRecipe}' nie istnieje.", exception.Message);			
		}

	}
}
