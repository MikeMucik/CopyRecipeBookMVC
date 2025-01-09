using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Domain.Interfaces
{
	public interface IRecipeRepository
	{
		int AddRecipe(Recipe recipe);
		void DeleteRecipe(int id);
		Recipe FindByName(string name);
		Recipe GetRecipeById(int id);
		void UpdateRecipe(Recipe recipe);
		IQueryable<Recipe> GetAllRecipes();
		IQueryable<Recipe> GetRecipesByCategory(int? categoryId, string? categoryName);
		IQueryable<Recipe> GetRecipesByDifficulty(int? difficultyId, string? difficultyName);
		IQueryable<Recipe> GetRecipesByIngredients(List<int>? ingredientsId, List<string>? ingredientsName);
		IQueryable<Recipe> GetRecipesByTime(int? timeId, int? timeAmount, string? timeUnit);		
		bool RecipeExist (int id);
	}
}
