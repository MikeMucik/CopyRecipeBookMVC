
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CopyRecipeBookMVC.Application.Test.Common
{
	public class DbContextFactory
	{
		//private static DbContextOptions<Context> _sharedOptions;//
		//private static bool _useSharedDatabase = false;
		//public static void UseSharedDatabase(bool useSharedDatabase)
		//{
		//	_useSharedDatabase = useSharedDatabase;
		//}
		//public static Context CreateContext()
		//{
		//	if (_useSharedDatabase)
		//	{
		//		if (_sharedOptions == null)
		//		{
		//			_sharedOptions = new DbContextOptionsBuilder<Context>()
		//				.UseInMemoryDatabase("SharedTestDatabase")
		//				.Options;
		//			using var context = new Context(_sharedOptions);
		//			context.Database.EnsureCreated();
		//			SeedDatabase(context);
		//		}
		//		return new Context(_sharedOptions);
		//	}
		//	else
		//	{
		//		var option = new DbContextOptionsBuilder<Context>()
		//			.UseInMemoryDatabase(Guid.NewGuid().ToString())
		//			.Options;

		//		var context = new Context(option);
		//		context.Database.EnsureCreated();
		//		SeedDatabase(context);
		//		return context;
		//	}
		//}

		public static Mock<Context> Create()
			{
			var options = new DbContextOptionsBuilder<Context>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			var mock = new Mock<Context>(options) { CallBase = true };
			var context = mock.Object;
			context.Database.EnsureCreated();
			SeedDatabase(context);
			return mock;
		}
		public static void SeedDatabase(Context context)
		{
			if (!context.Categories.Any())
			{
				var category = new Category
				{
					Id = 1,
					Name = "TestCategory",
				};
				context.Add(category);
				var category1 = new Category
				{
					Id = 2,
					Name = "TestCategory1",
				};
				context.Add(category1);
			}
			if (!context.Difficulties.Any())
			{
				var difficulty = new Difficulty
				{
					Id = 1,
					Name = "TestDifficulty"
				};
				context.Add(difficulty);
				var difficulty1 = new Difficulty
				{
					Id = 2,
					Name = "TestDifficulty1"
				};
				context.Add(difficulty1);
			}
			if (!context.Times.Any())
			{
				var time = new Time
				{
					Id = 1,
					Amount = 1,
					Unit = "TestTime"
				};
				context.Add(time);
			}
			if (!context.Ingredients.Any())
			{
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
			}
			if (!context.Units.Any())
			{
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
			}
			if (!context.Recipes.Any())
			{
				var recipe = new Recipe
				{
					Id = 1,
					Name = "Test",
					CategoryId = 1,
					DifficultyId = 1,					
					TimeId = 1,
					RecipeIngredient = new List<RecipeIngredient>(),
					Description = "Test",
				};
				context.Add(recipe);
				var recipe1 = new Recipe
				{
					Id = 20,
					Name = "Test1",
					CategoryId = 2,
					DifficultyId = 2,
					TimeId = 1,
					Description = "Test",
				};
				context.Add(recipe1);				
			}
			if (!context.RecipeIngredient.Any())
			{
				var recipeIngredient = new RecipeIngredient
				{
					RecipeId = 1,
					IngredientId = 1,
					UnitId = 1,
					Quantity = 1,
				};
				context.Add(recipeIngredient);
			}
			context.SaveChanges();
		}
		public static void Destroy(Context context)
		{
			context.Database.EnsureDeleted();
			context.Dispose();
		}
	}
}
