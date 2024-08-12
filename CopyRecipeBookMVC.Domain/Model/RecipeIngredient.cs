using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Domain.Model
{
	public class RecipeIngredient
	{
		public int Quantity { get; set; }
		public int RecipeId { get; set; }
		public virtual Recipe Recipe { get; set; }
		public int IngredientId { get; set; }
		public virtual Ingredient Ingredient { get; set; }
	}
}
