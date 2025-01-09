using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class ListRecipesByIngredientsDTO
	{
		public List<RecipeListForVm> RecipesByIngredients { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public List<string> IngredientName { get; set; }

		public int Count { get; set; }
	}
}
