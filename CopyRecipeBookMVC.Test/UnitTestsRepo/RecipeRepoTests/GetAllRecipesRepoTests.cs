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
	public class GetAllRecipesRepoTests
	{
		private readonly RecipeRepository _recipeRepo;
        public GetAllRecipesRepoTests(QueryTestFixtures fixtures)
        {
            var _context = fixtures.Context;
            _recipeRepo = new RecipeRepository(_context);
        }
        //[Fact]
        //public void Return_GetAllRecipes_When_Ok()
        //{
        //    //Arrange

        //    //Act
        //    var result = _recipeRepo.GetAllRecipes();
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.IsType<IQueryable<Recipe>>(result);
        //}
    }
}
