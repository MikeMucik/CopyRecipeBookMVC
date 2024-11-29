using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;

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
		[Fact]
		public void InvaliId_DeleteRecipe_ReturnThrowException()
		{
			//Arrange
			var id = 0;
			//Act
			void result()=>_recipeRepo.DeleteRecipe(id);
			//Arrange
			var exception = Assert.Throws<InvalidOperationException>(result);
            Assert.Equal($"Przepis o Id '{id}' nie istnieje.", exception.Message);
		}
	}
}
