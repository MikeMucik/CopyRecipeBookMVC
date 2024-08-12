using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Domain.Model
{
	public class Recipe
	{
        public int Id { get; set; }
		public string Name { get; set; }		
		public int CategoryId {  get; set; }
		public virtual Category Category { get; set; }
		public int DifficultyId { get; set; }
		public virtual Difficulty Difficulty { get; set; }
		public int TimeId { get; set; }
		public virtual Time Time { get; set; }
		public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }
		public string Description { get; set; }

	}
}
