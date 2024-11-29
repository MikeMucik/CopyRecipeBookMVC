using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CopyRecipeBookMVC.Application.Test.Common
{
	public class DbContextFactory
	{
		public static Mock<Context> Create()
		{
			var options = new DbContextOptionsBuilder<Context>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			var mock = new Mock<Context>(options) { CallBase = true };
			var context = mock.Object;
			context.Database.EnsureCreated();		

			var category = new Category
			{
				Id = 1,
				Name = "TestCategory",
			};
			context.Add(category);
			var difficulty = new Difficulty
			{
				Id = 1,
				Name = "TestDifficulty"
			};
			context.Add(difficulty);

			var category1 = new Category
			{
				Id = 2,
				Name = "TestCategory1",
			};
			context.Add(category1);
			var difficulty1 = new Difficulty
			{
				Id = 2,
				Name = "TestDifficulty1"
			};
			context.Add(difficulty1);
			var time = new Time
			{
				Id = 1,
				Amount = 1,
				Unit = "TestTime"
			};
			context.Add(time);
			
			var ingredientName = new Ingredient
			{
				Id = 1,
				Name = "TestIngredientName"
			};
			context.Add(ingredientName);
			var ingredientName1 = new Ingredient
			{
				Id = 2,
				Name = "TestIngredientName1"
			};
			context.Add(ingredientName1);
			var ingredientUnit = new Unit
			{
				Id = 1,
				Name = "TestIngredientUnit"
			};
			context.Add(ingredientUnit);
			var ingredientUnit1 = new Unit
			{
				Id = 2,
				Name = "TestIngredientUnit1"
			};
			context.Add(ingredientUnit1);
			var recipe = new Recipe
			{
				Id = 1,
				Name = "Test",
				CategoryId = category.Id,
				Category = category,
				DifficultyId = difficulty.Id,
				Difficulty = difficulty,
				TimeId = time.Id,
				Time = time,
				RecipeIngredient = new List<RecipeIngredient>(),
				Description = "Test",
			};
			context.Add(recipe);

			var recipe1 = new Recipe
			{
				Id = 20,
				Name = "Test1",
				CategoryId = 2,
				Category = category1,
				DifficultyId = 2,
				Difficulty = difficulty1,
				TimeId = 1,
				Time = time,
				Description = "Test",
			};
			context.Add(recipe1);
			var ingredient = new RecipeIngredient
			{
				Recipe = recipe,
				RecipeId = 1,
				Ingredient = ingredientName,
				IngredientId = 1,
				Unit = ingredientUnit,
				UnitId = 1,
				Quantity = 1,
			};
			context.Add(ingredient);
			context.SaveChanges();
			return mock;
		}
		public static void Destroy(Context context)
		{
			context.Database.EnsureDeleted();
			context.Dispose();
		}
	}
}
