using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.Unit;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Interfaces
{
    public interface IUnitService
    {
        ListUnitForListVm GetAllUnitsForList ();
       
        int AddUnit(IngredientForNewRecipeVm unit);
        int GetOrAddUnit(IngredientForNewRecipeVm ingredient);

	}
}
