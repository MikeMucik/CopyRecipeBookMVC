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

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	[Collection("QueryCollection")]
	public class GetRecipeToEditIntTests
	{
		private readonly RecipeService _recipeService;
		private readonly RecipeRepository _recipeRepo;
		private readonly IMapper _mapper;

		private readonly IRecipeIngredientService _recipeIngredientService;
		private readonly IIngredientService _ingredientService;
		private readonly IUnitService _unitService;

        public GetRecipeToEditIntTests(QueryTestFixtures fixtures)
        {
			var _context = fixtures.Context;
			var MappingConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			_mapper = MappingConfig.CreateMapper();
			_recipeRepo = new RecipeRepository(_context);
			var _ingredientRepo = new IngredientRepository(_context);
			_ingredientService = new IngredientService(_ingredientRepo, _mapper);
			var _unitRepo = new UnitRepository(_context);
			_unitService = new UnitService(_unitRepo, _mapper);
			var _recipeIngredientRepo = new RecipeIngredientRepository(_context);
			_recipeIngredientService = new RecipeIngredientService(_recipeIngredientRepo, _mapper);

			_recipeService = new RecipeService(_recipeRepo, _mapper, _recipeIngredientService);            
        }
		[Fact]
		public void FindByExistingId_GetRecipeToEdit_ReturnFilledNewRecipeVm()
		{
			//Arrange
			var existingRecipe = 1;
			//Act
			var result = _recipeService.GetRecipeToEdit(existingRecipe);
			//Assert
			Assert.NotNull(result);
			Assert.IsType<NewRecipeVm>(result);
		}

	}
}
