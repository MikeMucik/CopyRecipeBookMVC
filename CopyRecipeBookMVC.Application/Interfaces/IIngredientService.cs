using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface IIngredientService
	{

        void AddCompleteIngredients (RecipeIngredient recipeIngredient);
		int	AddIngredient (IngredientForNewRecipeVm ingredient);
        ListIngredientsForRecipeVm GetListIngredientForList ();
        //Ingredient GetIngredient(int id);
        int GetOrAddIngredient (IngredientForNewRecipeVm ingredient);
        void DeleteCompleteIngredients (int recipeId);
    }
}
