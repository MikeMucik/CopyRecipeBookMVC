using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class NewRecipeVm
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public int DifficultyId { get; set; }
		public int? TimeId { get; set; }
		public int? TimeAmount { get; set; }
		public string? TimeUnit { get; set; }
		public List<IngredientForNewRecipeVm> Ingredients { get; set; } = new List<IngredientForNewRecipeVm>();
		public string Description { get; set; }
	}
}
