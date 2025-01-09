using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeRepoTests
{
	public class DeleteRecipeRepoTests: CommandTestBase
	{
		private readonly RecipeRepository _recipeRepo;
        public DeleteRecipeRepoTests(): base()
        {
            _recipeRepo = new RecipeRepository(_context);
        }
        [Fact]
        public void ProperId_DeleteRecipe_RemoveRecipe()
        {
            //Arrange
            var id = 1; 
            //Act
             _recipeRepo.DeleteRecipe(id);
            //Arrange
            var deletedRecipe = _context.Recipes.FirstOrDefault(x => x.Id == id);
            Assert.Null(deletedRecipe);
        }		
	}
}
