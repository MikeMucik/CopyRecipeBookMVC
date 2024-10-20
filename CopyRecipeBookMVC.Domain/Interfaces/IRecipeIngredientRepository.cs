using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Domain.Interfaces
{
	public interface IRecipeIngredientRepository
	{
		void AddCompleteIngredients(RecipeIngredient recipeIngredient);
		void DeleteCompleteIngredient(RecipeIngredient item);
		IEnumerable<RecipeIngredient> GetAllIngredientsById(int recipeId);
	}
}
