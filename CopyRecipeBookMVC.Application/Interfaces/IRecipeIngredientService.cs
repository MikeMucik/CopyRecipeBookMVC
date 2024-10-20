using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Interfaces
{
    public interface IRecipeIngredientService
    {
		void AddCompleteIngredients(RecipeIngredient recipeIngredient);
		void DeleteCompleteIngredients(int recipeId);
	}
}
