using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeRepoTests
{
    [Collection("QueryCollection")]
	public class GetRecipeByIdRepoTests
	{
		private readonly RecipeRepository _recipeRepo;
        public GetRecipeByIdRepoTests(QueryTestFixtures fixtures)
        {
            var _context = fixtures.Context;
			_recipeRepo = new RecipeRepository(_context);
        }
		[Fact]
		public void ValidId_GetRecipeById_ReturnAllDates()
		{
			//Arrange
			var id = 1;
			//Act
			var result = _recipeRepo.GetRecipeById(id);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(id, result.Id);
			Assert.Equal(1, result.CategoryId);
		}
		[Fact]
		public void InvalidId_GetRecipeById_ReturnNull()
		{
			//Arrange
			var id = -1;
			//Act
			var result = _recipeRepo.GetRecipeById(id);
			//Assert
			Assert.Equal(null, result);
		}
	}
}
