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
	public class GetRecipesByDifficultyIntTests
	{
		private readonly RecipeService _recipeService;
		private readonly RecipeRepository _recipeRepo;
		private readonly IMapper _mapper;

        public GetRecipesByDifficultyIntTests(QueryTestFixtures fixtures)
        {
            var _context = fixtures.Context;
			var MappingConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			_mapper = MappingConfig.CreateMapper();
			_recipeRepo = new RecipeRepository(_context);
			_recipeService = new RecipeService(_recipeRepo, _mapper, null);
        }
		[Fact]
		public void ReturnByDifficulty_GetRecipesByDifficulty_ReturnList()
		{
			//Arrange
			int pageSize = 2;
			int pageNumber = 1;
			int difficulty = 2;
			//Act
			var result = _recipeService.GetRecipesByDifficulty(pageSize, pageNumber, difficulty);
			//Assert
			Assert.Equal(1, result.Count);
			Assert.NotNull(result);
			Assert.IsType<ListRecipesByDifficultyVm>(result);
		}
		[Fact]
		public void ReturnByNotExistingDifficulty_GetRecipesByDifficulty_ReturnEmptyList()
		{
			//Arrange
			int pageSize = 2;
			int pageNumber = 1;
			int difficulty = -1;
			//Act
			var result = _recipeService.GetRecipesByDifficulty(pageSize, pageNumber, difficulty);
			//Assert
			Assert.Equal([], result.RecipesByDifficulty);
			Assert.IsType<ListRecipesByDifficultyVm>(result);
		}		
	}
}
