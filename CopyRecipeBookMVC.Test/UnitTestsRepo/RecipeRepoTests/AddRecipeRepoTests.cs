using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Xunit.Sdk;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeRepoTests
{
	public class AddRecipeRepoTests : CommandTestBase
	{
		private readonly RecipeRepository _recipeRepo;
		public AddRecipeRepoTests(): base()
		{
			_recipeRepo = new RecipeRepository(_context);
		}
		[Fact]
		public void AddProperData_AddRecipe_ShouldAddToCollection()
		{
			//Arrange
			var repo = new Recipe
			{
				Id = 2,
				Name = "Name",
				Description = "Description",
			};
			//Act
			var result = _recipeRepo.AddRecipe(repo);
			//Assert
			//Assert.NotNull(result);
			Assert.Equal(2, result);
			Assert.IsType<int>(result);
		}
		[Fact]
		public void AddTheSameId_AddRecipe_ShouldNotAddToCollection()
		{
			//Arrange
			var repo = new Recipe
			{
				Id = 1,
				Name = "Name",
				Description = "Description",
			};
			//Act
			void result() => _recipeRepo.AddRecipe(repo);
			//Assert			
			var exception = Assert.Throws<InvalidOperationException>(result);
			Assert.Equal("Przepis o Id '1' już istnieje.", exception.Message);
		}
		//[Fact]
		//public void AddInvalidDataId_AddRecipe_ShouldNotAddToCollection()
		//{
		//	//Arrange
		//	var repo = new Recipe
		//	{
		//		Id = -1,		
		//		Name = "Name",
		//		Description = "Description",
		//	};
		//	//Act
		//	void result() => _recipeRepo.AddRecipe(repo);
		//	//Assert			
		//	var exception = Assert.Throws<ArgumentOutOfRangeException>(result);
		//	Assert.Equal("Id przepisu musi mieć wartość większą od zera (Parameter 'recipe')", exception.Message);
		//}
		[Fact]
		public void AddNullRecipe_AddRecipe_ShouldThrowsException()
		{
			//Arrange
			 Recipe repo = null;
			
			//Act
			void result() => _recipeRepo.AddRecipe(repo);
			//Assert				
			var exception = Assert.Throws<ArgumentNullException>(result);
			Assert.Equal("Nieprawidłowe dane (Parameter 'recipe')", exception.Message);
		}
	}
}
