using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Domain.Interfaces
{
	public interface IIngredientRepository
	{
		int AddIngredient (Ingredient ingredient);
		void AddCompleteIngredients (RecipeIngredient recipeIngredient);
		IQueryable<Ingredient> GetAllIngredients ();
		Ingredient GetIngredientById (int id);
		int AddUnit (Unit unit);
		IQueryable<Unit> GetAllUnits ();
		Unit GetUnitById (int id);
        IEnumerable<RecipeIngredient> GetAllIngredientsById(int recipeId);
        void DeleteCompleteIngredient(RecipeIngredient item);
		Ingredient ExistingIngredient(string name);
		Unit ExistingUnit(string name);
    }
}
