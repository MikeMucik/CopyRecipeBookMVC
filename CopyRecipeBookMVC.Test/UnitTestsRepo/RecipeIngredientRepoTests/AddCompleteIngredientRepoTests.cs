using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsRepo.RecipeIngredientRepoTests
{
	public class AddCompleteIngredientRepoTests :CommandTestBase
	{
		private readonly RecipeIngredientRepository _recipeIngredientRepo;
        public AddCompleteIngredientRepoTests() :base()
        {
            _recipeIngredientRepo = new RecipeIngredientRepository(_context);
        }
        [Fact]
        public void AddProperData_AddCompleteIngredient_AddedToRecipe()
        {
            //Arrange
            var recipeIng = new RecipeIngredient
            {
                RecipeId = 1,
                IngredientId = 3,
                UnitId = 4,
                Quantity = 5
            };
            //Act
            _recipeIngredientRepo.AddCompleteIngredients(recipeIng);
            //Assert            
            var result = _context.RecipeIngredient.FirstOrDefault(r=>r.RecipeId == recipeIng.RecipeId && r.IngredientId == recipeIng.IngredientId);
            Console.WriteLine(result);
            Assert.Equal(3, result.IngredientId );
            Assert.Equal(5, result.Quantity );
            Assert.Equal(4, result.UnitId );
        }
		[Fact]
		public void AddNullData_AddCompleteIngredient_ThrowException()
		{
			//Arrange
			
			//Act
			void result()=>_recipeIngredientRepo.AddCompleteIngredients(null);
			//Assert
			var exception = Assert.Throws<ArgumentNullException>(result);
			Assert.Equal("Błędne dane (Parameter 'recipeIngredient')", exception.Message);
			
		}
		[Fact]
		public void AddNullIngredientData_AddCompleteIngredient_ThrowException()
		{
			//Arrange
			var recipeIng = new RecipeIngredient
			{
				RecipeId = 1,
				IngredientId = 0,
				UnitId = 4,
				Quantity = 5
			};
			//Act
			void result() => _recipeIngredientRepo.AddCompleteIngredients(recipeIng);
			//Assert
			var exception = Assert.Throws<ArgumentNullException>(result);
			Assert.Equal("Błędne dane (Parameter 'recipeIngredient')", exception.Message);
		}
		[Fact]
		public void AddNullUnitData_AddCompleteIngredient_ThrowException()
		{
			//Arrange
			var recipeIng = new RecipeIngredient
			{
				RecipeId = 1,
				IngredientId = 3,
				UnitId = 0,
				Quantity = 5
			};
			//Act
			void result() => _recipeIngredientRepo.AddCompleteIngredients(recipeIng);
			//Assert
			var exception = Assert.Throws<ArgumentNullException>(result);
			Assert.Equal("Błędne dane (Parameter 'recipeIngredient')", exception.Message);
		}
		[Fact]
		public void AddNullQuantityData_AddCompleteIngredient_ThrowException()
		{
			//Arrange
			var recipeIng = new RecipeIngredient
			{
				RecipeId = 1,
				IngredientId = 3,
				UnitId = 4,
				Quantity = 0
			};
			//Act
			void result() => _recipeIngredientRepo.AddCompleteIngredients(recipeIng);
			//Assert
			var exception = Assert.Throws<ArgumentNullException>(result);
			Assert.Equal("Błędne dane (Parameter 'recipeIngredient')", exception.Message);
		}
	}
}
