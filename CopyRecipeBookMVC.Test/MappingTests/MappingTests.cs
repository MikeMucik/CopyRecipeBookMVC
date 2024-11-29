using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Test.MappingTests
{
	public class MappingTests
	{
		private readonly IMapper _mapper;

		public MappingTests()
		{
			var mappingConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			_mapper = mappingConfig.CreateMapper();
		}
		[Fact]
		public void Should_Map_IngredientForNewRecipe_To_Ingredient()
		{
			//Arrange
			var ingredientNameVm = new IngredientForNewRecipeVm
			{
				NewIngredientName = "Bread",
			};
			//Act
			var ingredientName = _mapper.Map<Ingredient>(ingredientNameVm);
			//Assert
			Assert.Equal("Bread", ingredientName.Name);			
		}
		[Fact]
		public void Should_Map_UnitForNewRecipe_To_Ingredient()
		{
			//Arrange
			var ingredientUnitVm = new IngredientForNewRecipeVm
			{
				NewIngredientUnit = "ml",
			};
			//Act
			var ingredientUnit = _mapper.Map<Unit>(ingredientUnitVm);
			//Assert
			Assert.Equal("ml", ingredientUnit.Name);
		}
		[Fact]
		public void Should_Map_NewRecipe_To_Recipe()
		{
			//Arrange
			var recipeVm = new NewRecipeVm
			{
				Id = 10,
				Name = "TestMapping",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				TimeAmount = 1,
				TimeUnit = "h",
				Description = "TestMapping",
				NumberOfIngredients = 1,
				Ingredients = new List<IngredientForNewRecipeVm>
				{
					new IngredientForNewRecipeVm  { IngredientName = 1, IngredientUnit = 1, Quantity = 1  }
				}

			};
			//Act
			var recipe = _mapper.Map<Recipe>(recipeVm);
			//Assert
			Assert.Equal("TestMapping", recipe.Name);
			Assert.Equal(10, recipe.Id);
			Assert.Equal(1, recipe.CategoryId);
			Assert.Equal(1, recipe.DifficultyId);
			Assert.Equal(1, recipe.TimeId);
			Assert.Equal("TestMapping", recipe.Description);
		}
		[Fact]
		public void Should_Map_NewRecipe_To_Time()
		{
			//Arrange
			var recipeVm = new NewRecipeVm
			{
				Id = 10,
				Name = "TestMapping",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				TimeAmount = 1,
				TimeUnit = "h",
				Description = "TestMapping",
				NumberOfIngredients = 1,
				Ingredients = new List<IngredientForNewRecipeVm>
				{
					new IngredientForNewRecipeVm  { IngredientName = 1, IngredientUnit = 1, Quantity = 1  }
				}
			};
			//Act
			var recipe = _mapper.Map<Time>(recipeVm);
			//Assert
			Assert.Equal("h", recipe.Unit);			
			Assert.Equal(1, recipe.Amount);			
		}
		[Fact]
		public void Should_Map_CompleteIngredientForNewRecipe_To_RecipeIngredient()
		{
			//Arrange
			var ingredientVm = new IngredientForNewRecipeVm
			{
				IngredientName = 10,
				Quantity = 20,
				IngredientUnit = 30,
			};
			//Act
			var ingredient = _mapper.Map<RecipeIngredient>(ingredientVm);
			//Assert
			Assert.Equal(10, ingredient.IngredientId);
			Assert.Equal(20, ingredient.Quantity);
			Assert.Equal(30, ingredient.UnitId);
		}
	}
}
