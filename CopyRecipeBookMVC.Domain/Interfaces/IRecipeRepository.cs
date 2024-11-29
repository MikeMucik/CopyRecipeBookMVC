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
		IQueryable<Recipe> GetRecipesByCategory(int categoryId);
		IQueryable<Recipe> GetRecipesByDifficulty(int difficultyId);
		IQueryable<Recipe> GetRecipesByIngredients(List<int> ingredients);
		IQueryable<Recipe> GetRecipesByTime(int timeId);		
	}
}
