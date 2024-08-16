using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface IRecipeService
	{
		int AddRecipe(NewRecipeVm recipe);
		void DeleteRecipe(int id);
		ListRecipeForListVm GetAllRecipesForList(int pageSize, int pageNumber, string searchString);
		RecipeDetailsVm GetRecipe(int id);
		int UpdaterRecipe(Recipe recipe);
		ListRecipesByCategoryVm GetRecipesByCategory(int pageSize, int pageNumber, string categoryId);
		ListRecipeForListVm GetRecipesByDifficulty(int difficultyId);

	}
}
