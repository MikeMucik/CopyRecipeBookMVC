using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface IIngredientService
	{
		int	AddIngredient (IngredientForNewRecipeVm ingredient);
        ListIngredientsForRecipeVm GetListIngredientForList ();       
        int GetOrAddIngredient (IngredientForNewRecipeVm ingredient);      
		List<SelectListItem> GetIngredientSelectList();
    }
}
