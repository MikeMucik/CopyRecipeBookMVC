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
		Recipe GetRecipeById(int id);
		int UpdateRecipe(Recipe recipe);
		IQueryable<Recipe> GetAllRecipes();
		IQueryable<Recipe> GetRecipesByCategory(int categoryId);
		IQueryable<Recipe> GetRecipesByDifficulty(int difficultyId);
		IQueryable<Recipe> GetRecipesByTime(int timeId);
		IEnumerable<Category> GetAllCategories();
		IEnumerable<Difficulty> GetAllDifficulties();
	}
}
