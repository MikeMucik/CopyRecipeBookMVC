using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class RecipeDetailsVm
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string Difficulty { get; set; }
		public string Time { get; set; }
		// 
		public string Description { get; set; }
		

	}
}
