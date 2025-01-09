using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeRepoTests
{
	public class UpdateRecipeRepoTests: CommandTestBase
	{
		private readonly DbContextOptions<Context> _contextOptions;
		private readonly RecipeRepository _recipeRepo;
        public UpdateRecipeRepoTests(): base()
        {
			_contextOptions = new DbContextOptionsBuilder<Context>()
			.UseInMemoryDatabase("TestDatabase")
			.Options;
			
			_recipeRepo = new RecipeRepository(_context);			
		}
		[Fact]
		public void AddProperData_UpdateRecipe_ReturnUpdatedRecipe()
		{
			//Arrange
			using var arrangeContext = new Context(_contextOptions);
			arrangeContext.Recipes.Add(new Recipe
			{
				Id = 1,
				Name = "Original Name",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Original Description",
			});
			arrangeContext.SaveChanges();
			// Act: Aktualizujemy rekord w nowym kontekście
			using (var actContext = new Context(_contextOptions))
			{
				var repo = new RecipeRepository(actContext);
				repo.UpdateRecipe(new Recipe
				{
					Id = 1, // To samo Id
					Name = "Updated Name",
					CategoryId = 2,
					DifficultyId = 2,
					TimeId = 2,
					Description = "Updated Description",
				});
			}
			// Assert: Sprawdzamy wynik w kolejnym kontekście
			using (var assertContext = new Context(_contextOptions))
			{
				var result = assertContext.Recipes.FirstOrDefault(x => x.Id == 1);

				Assert.NotNull(result);
				Assert.Equal("Updated Name", result.Name);
				Assert.Equal(2, result.CategoryId);
				Assert.Equal(2, result.DifficultyId);
				Assert.Equal(2, result.TimeId);
				Assert.Equal("Updated Description", result.Description);
			}
		}
		//[Fact]
		//public void AddNotExistingRecipe_UpdateRecipe_ThrowException()
		//{
		//	//Arrange
		//	var updatedRecipe = new Recipe
		//	{
		//		Id = -999, 
		//		Name = "Updated Name",
		//		CategoryId = 2,
		//		DifficultyId = 2,
		//		TimeId = 2,
		//		Description = "Updated Description",
		//	};
		//	// Act
		//	void result() => _recipeRepo.UpdateRecipe(updatedRecipe);

		//	// Assert
		//	var exception = Assert.Throws<InvalidOperationException>(result);
		//	Assert.Equal($"Przepis o Id '{updatedRecipe.Id}' nie istnieje", exception.Message);
		//}
		//[Fact]
		//public void AddInvalidData_UpdateRecipe_ThrowException()
		//{
		//	//Arrange
		//	var updatedRecipe = new Recipe
		//	{
		//		Id =-1,
		//		Name = "Updated Name",
		//		CategoryId = 2,
		//		DifficultyId = 2,
		//		TimeId = 2,
		//		Description = "Updated Description",
		//	};
		//	// Act
		//	void result() => _recipeRepo.UpdateRecipe(updatedRecipe);

		//	// Assert
		//	var exception = Assert.Throws<InvalidDataException>(result);
		//	Assert.Equal("Numer przepisu musi być większy od zera", exception.Message);
		//}
		//[Fact]
		//public void AddNullData_UpdateRecipe_ThrowException()
		//{
		//	//Arrange
		//	// Act
		//	void result() => _recipeRepo.UpdateRecipe(null);

		//	// Assert
		//	var exception = Assert.Throws<ArgumentNullException>(result);
		//	Assert.Equal("Nieprawidłowe dane (Parameter 'recipe')", exception.Message);
		//}		
	}
}
