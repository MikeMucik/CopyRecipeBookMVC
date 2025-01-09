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
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
        }
        public Recipe FindByName(string name)
        {
            return _context.Recipes.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }
        public Recipe GetRecipeById(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var recipe = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Difficulty)
                .Include(r => r.Time)
                .Include(r => r.RecipeIngredient)
                    .ThenInclude(i => i.Ingredient)
                .Include(r => r.RecipeIngredient)
                    .ThenInclude(i => i.Unit)
                .FirstOrDefault(r => r.Id == id);
            return recipe;
        }
        public void UpdateRecipe(Recipe recipe)
        {
            _context.Attach(recipe);
            _context.Entry(recipe).Property(nameof(recipe.Name)).IsModified = true;
            _context.Entry(recipe).Property(nameof(recipe.CategoryId)).IsModified = true;
            _context.Entry(recipe).Property(nameof(recipe.DifficultyId)).IsModified = true;
            _context.Entry(recipe).Property(nameof(recipe.TimeId)).IsModified = true;
            _context.Entry(recipe).Property(nameof(recipe.Description)).IsModified = true;
            // Aktualizacja składników dla DTO
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
        public IQueryable<Recipe> GetRecipesByCategory(int? categoryId, string? categoryName)
        {
            var result = _context.Recipes
                .Include(r => r.Category)
                .AsQueryable();
            if (categoryId.HasValue && categoryId > 0)
            {
                result = result.Where(r => r.CategoryId == categoryId);
            }
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                result = result.Where(r => r.Category.Name == categoryName && r.Category != null);
            }
            return result;
        }
        public IQueryable<Recipe> GetRecipesByDifficulty(int? difficultyId, string? difficultyName)
        {
            var result = _context.Recipes
                .Include(r => r.Difficulty)
                .AsQueryable(); ;
            if (difficultyId.HasValue && difficultyId < 0)
            {
                result = result.Where(r => r.DifficultyId == difficultyId);
            }
            if (!string.IsNullOrWhiteSpace(difficultyName))
            {
                result = result.Where(r => r.Difficulty.Name == difficultyName && r.Difficulty != null);
            }
            return result;
        }
        public IQueryable<Recipe> GetRecipesByTime(int? timeId, int? timeAmount, string? timeUnit)
        {
            var result = _context.Recipes
                .Include(r => r.Time)
                .AsQueryable();
            if (timeId.HasValue && timeId > 0)
            {
                result = result.Where(r => r.TimeId == timeId);
            }
            if ((timeAmount.HasValue && timeAmount < 0) && !string.IsNullOrWhiteSpace(timeUnit))
            {
                result = result.Where(r => r.Time.Amount == timeAmount && r.Time.Unit == timeUnit && r.Time.Unit != null);
            }
            return result;
        }
        public IQueryable<Recipe> GetRecipesByIngredients(List<int>? ingredientsIds, List<string>? ingredientsName)
        {        
            if (ingredientsIds != null && ingredientsIds.Count != 0)
            {
                //return _context.Recipes
                //	.Where(r => ingredientsIds.All(id => r.RecipeIngredient.Any(ri => ri.IngredientId == id)));
                // Wersja dla testów
                var recipeIds = _context.RecipeIngredient
                    .Where(ri => ingredientsIds.Contains(ri.IngredientId))
                    .GroupBy(ri => ri.RecipeId)
                    .Where(group => group.Select(ri => ri.IngredientId).Distinct().Count() == ingredientsIds.Count)
                    .Select(group => group.Key);
                return _context.Recipes.Where(r => recipeIds.Contains(r.Id));
            }
            if (ingredientsName != null && ingredientsName.Count != 0)
            {
                //return _context.Recipes
                //.Include(r => r.RecipeIngredient)
                //	.ThenInclude(i => i.Ingredient)
                //.Where(r => ingredientsName.All(name =>
                //	r.RecipeIngredient.Any(ri => ri.Ingredient.Name == name)));
                var recipeNames = _context.RecipeIngredient
                    .Where(ri => ingredientsName.Contains(ri.Ingredient.Name))
                    .GroupBy(ri => ri.RecipeId)
                    .Where(group => group.Select(ri => ri.Ingredient.Name).Distinct().Count() == ingredientsName.Count)
                    .Select(group => group.Key);
                return _context.Recipes
                    .Include(r => r.RecipeIngredient)
                    .ThenInclude(ri => ri.Ingredient)
                    .Where(r => recipeNames.Contains(r.Id));
            }
            return Enumerable.Empty<Recipe>().AsQueryable();
        }
        public bool RecipeExist(int id)
        {
            return _context.Recipes.Any(r => r.Id == id);
        }
    }
}
