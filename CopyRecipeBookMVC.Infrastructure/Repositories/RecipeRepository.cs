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
			if (recipe == null)
			{
				throw new ArgumentNullException(nameof(recipe), "Nieprawidłowe dane");
			}
			//if (recipe.Id <= 0)
			//{
			//	throw new ArgumentOutOfRangeException(nameof(recipe), "Id przepisu musi mieć wartość większą od zera");
			//}
			if (_context.Recipes.Any(r => r.Id == recipe.Id))
			{
				throw new InvalidOperationException($"Przepis o Id '{recipe.Id}' już istnieje.");
			}

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
			else
			{
				throw new InvalidOperationException($"Przepis o Id '{id}' nie istnieje.");
			}
		}
		public Recipe FindByName(string name)
		{
			return _context.Recipes.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
		}
		public Recipe GetRecipeById(int id)
		{
			var recipe = _context.Recipes
				.Include(r => r.Category)
				.Include(r => r.Difficulty)
				.Include(r => r.Time)
				.Include(r => r.RecipeIngredient)
					.ThenInclude(i => i.Ingredient)
				.Include(r => r.RecipeIngredient)
					.ThenInclude(i => i.Unit)
				.FirstOrDefault(r => r.Id == id);
			if (recipe == null)
			{
				throw new InvalidOperationException($"Przepis o Id '{id}' nie istnieje.");
			}
			return recipe;
		}
		public void UpdateRecipe(Recipe recipe)
		{
			if (recipe == null)
			{
				throw new ArgumentNullException(nameof(recipe), "Nieprawidłowe dane");
			}
			//if (recipe.Id <= 0)
			//{
			//	throw new InvalidDataException("Numer przepisu musi być większy od zera");
			//}
			
			if (!_context.Recipes.Any(r=>r.Id == recipe.Id))
			{
				throw new InvalidOperationException($"Przepis o Id '{recipe.Id}' nie istnieje");
			}
			_context.Attach(recipe);
			_context.Entry(recipe).Property(nameof(recipe.Name)).IsModified = true;
			_context.Entry(recipe).Property(nameof(recipe.CategoryId)).IsModified = true;
			_context.Entry(recipe).Property(nameof(recipe.DifficultyId)).IsModified = true;
			_context.Entry(recipe).Property(nameof(recipe.TimeId)).IsModified = true;
			_context.Entry(recipe).Property(nameof(recipe.Description)).IsModified = true;
			// Aktualizacja składników dl DTO
			var existingIngredients = _context.RecipeIngredient
				.Where(ri => ri.RecipeId == recipe.Id)
				.ToList();

			foreach (var ingredient in existingIngredients)
			{
				if (!recipe.RecipeIngredient.Any(i => i.IngredientId == ingredient.IngredientId && i.UnitId == ingredient.UnitId))
				{
					_context.RecipeIngredient.Remove(ingredient);
				}
			}
			
			foreach (var ingredient in recipe.RecipeIngredient)
			{
				if (!existingIngredients.Any(i => i.IngredientId == ingredient.IngredientId && i.UnitId == ingredient.UnitId))
				{
					_context.RecipeIngredient.Add(ingredient);
				}
			}
			_context.SaveChanges();
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
		public IQueryable<Recipe> GetRecipesByTime(int timeId) //To jest na razie nie wykorzystane
		{
			return _context.Recipes.Where(r => r.TimeId == timeId);
		}
		public IQueryable<Recipe> GetRecipesByIngredients(List<int> ingredientsIds)
		{
			//return _context.Recipes
			//	.Where(r => ingredientsIds.All(id => r.RecipeIngredient.Any(ri => ri.IngredientId == id)));
			// Wersja dla testów
			var recipeIds = _context.RecipeIngredient
				.Where(ri => ingredientsIds.Contains(ri.IngredientId))
				.GroupBy(ri => ri.RecipeId)
				.Where(group => group.Select(ri => ri.IngredientId).Distinct().Count() == ingredientsIds.Count)
				.Select(group => group.Key);

			// Pobierz przepisy o wybranych ID
			return _context.Recipes.Where(r => recipeIds.Contains(r.Id));
		}
	}
}
