using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Domain.Model
{
	public class Time
	{
		public int Id { get; set; }
		public int Amount { get; set; }
		public string Unit { get; set; }
		public ICollection<Recipe> Recipes { get; set; }
	}
}
