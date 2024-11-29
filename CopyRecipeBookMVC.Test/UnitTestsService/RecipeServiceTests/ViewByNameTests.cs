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
using CopyRecipeBookMVC.Infrastructure;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsService.RecipeServiceTests
{
	[Collection("QueryCollection")]
	public class ViewByNameTests
	{		
        private readonly RecipeService _recipeService;		
        public ViewByNameTests(QueryTestFixtures fixtures)
        {            
			var _context = fixtures.Context;
            var _recipeRepo = new RecipeRepository(_context);
			var _recipeIngredientRepo = new RecipeIngredientRepository(_context);
            var _recipeIngredientService = new RecipeIngredientService(_recipeIngredientRepo, fixtures.Mapper);
			_recipeService = new RecipeService(_recipeRepo, fixtures.Mapper, _recipeIngredientService);
        }
        [Fact]
        public void Return_GetAllRecipesForList_ShowListOfRecipes()
        {
            //Arrange
            int pageSize = 12;
            int pageNumber = 1;
            string searchString = "";
            //Act
            var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			//Assert			
            Assert.NotNull(result);
            Assert.NotNull(result.Recipes);
            var recipe = result.Recipes.FirstOrDefault(r=>r.Name=="Test");
            Assert.IsType<ListRecipeForListVm>(result);
        }
		[Fact]
		public void Return_GetAllRecipesForList_ShowEmptyListOfRecipes()
		{
			//Arrange
			int pageSize = 12;
			int pageNumber = 1;
			string searchString = "S";
			//Act
			var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			//Assert			
			Assert.Equal([], result.Recipes);			
		}
	}
}
