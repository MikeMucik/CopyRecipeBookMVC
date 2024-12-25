using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
	public class RecipeIngredientRepository : IRecipeIngredientRepository
	{
		private readonly Context _context;
		public RecipeIngredientRepository(Context context)
		{
			_context = context;
		}
		public void AddCompleteIngredients(RecipeIngredient recipeIngredient)
		{
			if (recipeIngredient == null || recipeIngredient.Quantity <= 0 ||
				(recipeIngredient.IngredientId == 0 && string.IsNullOrEmpty(recipeIngredient.Ingredient?.Name))
				|| (recipeIngredient.UnitId == 0 && string.IsNullOrEmpty(recipeIngredient.Unit?.Name)))				
			{
				throw new ArgumentNullException(nameof(recipeIngredient), "Błędne dane");
			}

			_context.RecipeIngredient.Add(recipeIngredient);
			_context.SaveChanges();
		}
		public void DeleteCompleteIngredient(RecipeIngredient item)
		{
			_context.RecipeIngredient.Remove(item);
			_context.SaveChanges();
		}
		public IEnumerable<RecipeIngredient> GetAllIngredientsById(int recipeId)
		{
			return _context.RecipeIngredient.Where(ri => ri.RecipeId == recipeId)
											.ToList();
		}
	}
}
