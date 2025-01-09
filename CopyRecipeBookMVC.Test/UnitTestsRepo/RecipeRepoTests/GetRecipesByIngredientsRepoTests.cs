using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeRepoTests
{
	[Collection("QueryCollection")]
	public class GetRecipesByIngredientsRepoTests
	{
		private readonly RecipeRepository _recipeRepo;
        public GetRecipesByIngredientsRepoTests(QueryTestFixtures fixtures)
        {
            var _context = fixtures.Context;
            _recipeRepo = new RecipeRepository(_context);
        }
        [Fact]
        public void PutIngredients_GetRecipeByIngredients_ReturnEmptyList()
        {
            //Arrange
            List<int> ints = new List<int>() { 1,2};
            //Act
            var result = _recipeRepo.GetRecipesByIngredients(ints, null);
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IQueryable<Recipe>>(result);
			Assert.Equal(0, result.Count());
		}
		[Fact]
		public void PutIngredientId_GetRecipeByIngredients_ReturnList()
		{
			//Arrange
			List<int> ints = new List<int> { 1};
			//Act
			var result = _recipeRepo.GetRecipesByIngredients(ints, null);
			//Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<IQueryable<Recipe>>(result);
			Assert.Equal(1, result.Count());
		}
	}
}
