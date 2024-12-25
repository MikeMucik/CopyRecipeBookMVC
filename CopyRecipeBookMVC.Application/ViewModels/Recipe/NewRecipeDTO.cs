using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class NewRecipeDTO :IMapFrom<Domain.Model.Recipe>,
								IMapFrom<Domain.Model.Time>
	{
		
		public int Id { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string Difficulty { get; set; }
		public string? Time { get; set; }
		public decimal TimeAmount { get; set; } 
		public string TimeUnit { get; set; }
		public List<IngredientForRecipeVm> Ingredients { get; set; } = new List<IngredientForRecipeVm>();
		public string Description { get; set; }
		
	}
}
