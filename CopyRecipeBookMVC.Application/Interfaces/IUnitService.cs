using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Application.ViewModels.Unit;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Interfaces
{
    public interface IUnitService
    {
        List<SelectListItem> GetUnitsForSelectList ();       
        int AddUnit(IngredientForNewRecipeVm unit);
        int GetOrAddUnit(IngredientForNewRecipeVm ingredient);
	}
}
