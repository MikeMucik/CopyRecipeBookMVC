using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.Ingredient
{
	public class IngredientForNewRecipeVm
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Quantity { get; set; }
		public string Unit { get; set; }

		public string NewIngredientName { get; set; }
		public int NewIngredientQuantity { get; set; }
		public string NewIngredientUnit { get; set; }

	}
}
