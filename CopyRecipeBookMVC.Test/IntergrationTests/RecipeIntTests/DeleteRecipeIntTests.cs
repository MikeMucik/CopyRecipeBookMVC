using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	public class DeleteRecipeIntTests :RecipeIntegrationCommand
		
	{		
        [Fact]
        public void ExistingRecipe_DeleteRecipe_DeleteOk()
        {
            //Arrange
            int existingRecipe = 1;
            //Act
            _recipeService.DeleteRecipe(existingRecipe);
            //Assert
            var result = _context.Recipes.FirstOrDefault(r=> r.Id == existingRecipe);
            Assert.Null(result);
        }
		[Fact]
		public void NotExistingRecipe_DeleteRecipe_BaseException()
		{
			//Arrange
			int existingRecipe = 1000; //notexisting
			//Act
			void result()=> _recipeService.DeleteRecipe(existingRecipe);
			//Assert
			var exception = Assert.Throws<InvalidOperationException>(result);
			Assert.Equal($"Przepis o Id '{existingRecipe}' nie istnieje.", exception.Message);
		}
	}
}
