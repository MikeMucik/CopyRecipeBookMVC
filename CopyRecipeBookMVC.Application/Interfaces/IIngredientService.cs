using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface IIngredientService
	{
		int	AddIngredient (Ingredient ingredient);
		List<Ingredient> GetAllIngredients ();
		Ingredient GetIngredient (int id);
		int AddUnit (Unit unit);
		List<Unit> GetAllUnits ();
		Unit GetUnit (int id);
	}
}
