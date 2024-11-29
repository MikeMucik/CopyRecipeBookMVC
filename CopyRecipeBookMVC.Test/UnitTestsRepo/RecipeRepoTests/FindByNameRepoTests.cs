using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeRepoTests
{
    [Collection("QueryCollection")]
    public class FindByNameRepoTests
    {
        private readonly RecipeRepository _recipeRepo;
        public FindByNameRepoTests(QueryTestFixtures fixtures)
        {
            var _context = fixtures.Context;
            _recipeRepo = new RecipeRepository(_context);
        }
		[Fact]
		public void Return_FindByName_Return_ExistingRecipeByName()
		{
			//Arrange
			var repo = "test";
			//Act
			var result = _recipeRepo.FindByName(repo);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Id);
			Assert.IsType<Recipe>(result);
		}
		[Fact]
		public void Return_FindByName_ReturnNull_NotExistingRecipe()
		{
			//Arrange
			var repo = "testy";
			//Act
			var result = _recipeRepo.FindByName(repo);
			//Assert
			Assert.Null(result);
		}
	}
}
