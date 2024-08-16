using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
	public class RecipeRepository : IRecipeRepository
	{
		private readonly Context _context;
		public RecipeRepository(Context context)
		{
			_context = context;
		}
		public int AddRecipe(Recipe recipe)
		{
			_context.Recipes.Add(recipe);
			_context.SaveChanges();
			return recipe.Id;
		}

		public void DeleteRecipe(int id)
		{
			var recipe = _context.Recipes.Find(id);
			if (recipe != null)
			{
				_context.Recipes.Remove(recipe);
				_context.SaveChanges();
			}
		}

		public Recipe GetRecipeById(int id)
		{
			var recipe = _context.Recipes
				.Include(r => r.Category)
				.Include(r => r.Difficulty)
				.Include(r => r.Time)
				.Include(r => r.RecipeIngredient)
					.ThenInclude(i => i.Ingredient)
				.Include(r=> r.RecipeIngredient)
					.ThenInclude(i => i.Unit)
				.FirstOrDefault(r => r.Id == id);
			
				
			if (recipe == null)
			{
				Console.WriteLine("Not Found");
			}
			return recipe;
		}

		public int UpdateRecipe(Recipe recipe)
		{
			throw new NotImplementedException();
		}

		public IQueryable<Recipe> GetAllRecipes()
		{
			return _context.Recipes;
		}

		public IQueryable<Recipe> GetRecipesByCategory(int categoryId)
		{
			return _context.Recipes.Where(r => r.CategoryId == categoryId);
		}

		public IQueryable<Recipe> GetRecipesByDifficulty(int difficultyId)
		{
			return _context.Recipes.Where(r => r.DifficultyId == difficultyId);
		}

		public IQueryable<Recipe> GetRecipesByTime(int timeId)
		{
			return _context.Recipes.Where(r=> r.TimeId == timeId);
		}

		public IEnumerable<Category> GetAllCategories()	
		{
			return _context.Categories.ToList();
		}

		public IEnumerable<Difficulty> GetAllDifficulties()
		{
			return _context.Difficulties.ToList();
		}

		
	}
}
