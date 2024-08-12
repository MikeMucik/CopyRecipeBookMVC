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
		IEnumerable<Ingredient> GetAllIngredients ();
		Ingredient GetIngredientById (int id);
		int AddUnit (Unit unit);
		IEnumerable<Unit> GetAllUnits ();
		Unit GetUnitById (int id);
	}
}
