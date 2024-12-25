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
		//int? CheckIfRecipeExists(string recipeName);
		int AddRecipeApi(NewRecipeDTO newRecipe);
		//int AddRecipeApi(NewRecipeDTO newRecipe);
		void DeleteRecipe(int id);
		bool CheckNameForRecipe(string name);
		int? TryAddRecipe(NewRecipeVm model);
		ListRecipeForListVm GetAllRecipesForList(int pageSize, int pageNumber, string searchString);
		RecipeDetailsVm GetRecipe(int id);
		//RecipeDetailsVm GetRecipeDTO(int id);
		void UpdateRecipe(NewRecipeVm recipe);
		bool UpdateRecipeApi(NewRecipeDTO recipeUpdated);
		ListRecipesByCategoryVm GetRecipesByCategory(int pageSize, int pageNumber, int categoryId);
		ListRecipesByDifficultyVm GetRecipesByDifficulty(int pageSize, int pageNumber, int difficultyId);
		NewRecipeVm GetRecipeToEdit(int id);
		ListRecipesByIngredientsVm GetRecipesByIngredients(int pageSize, int pageNumber, List<int> IngredientIds);
	}
}
