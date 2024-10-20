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
    class RecipeIngredientRepository :IRecipeIngredientRepository
    {
		private readonly Context _context;
		public RecipeIngredientRepository(Context context)
		{
			_context = context;
		}
		public void AddCompleteIngredients(RecipeIngredient recipeIngredient)
		{
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
