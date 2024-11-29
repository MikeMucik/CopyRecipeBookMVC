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
	public class TryAddRecipeIntTests		
	{
		private readonly RecipeService _recipeService;
		private readonly RecipeRepository _recipeRepo;
		private readonly IMapper _mapper;

        public TryAddRecipeIntTests(QueryTestFixtures fixtures) 			
        {
			var _context = fixtures.Context;
			var MapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			_mapper = MapperConfig.CreateMapper();

			_recipeRepo = new RecipeRepository(_context);

			_recipeService = new RecipeService(_recipeRepo, _mapper, null);            
        }
		[Fact]
		public void ExistingRecipe_TryAddRecipe_ReturnIdExisting()
		{
			//Arrange
			var tryRecipe = new NewRecipeVm
			{
				Name = "Test",
			};
			//Act
			var result = _recipeService.TryAddRecipe(tryRecipe);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(1, result);
		}
		[Fact]
		public void NotExistingRecipe_TryAddRecipe_ReturnNull()
		{
			//Arrange
			var tryRecipe = new NewRecipeVm
			{
				Name = "NotTest",
			};
			//Act
			var result = _recipeService.TryAddRecipe(tryRecipe);
			//Assert
			Assert.Null(result);
		}
	}
}
