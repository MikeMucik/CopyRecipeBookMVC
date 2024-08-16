using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Ingredient
{
	public class IngredientForRecipeVm :IMapFrom<Domain.Model.RecipeIngredient>
	{
		
		public string Name { get; set; }
		public decimal Quantity { get; set; }
		public string Unit { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<Domain.Model.RecipeIngredient, IngredientForRecipeVm>()
				.ForMember(i => i.Name, opt => opt.MapFrom(o => o.Ingredient.Name))
				.ForMember(i => i.Quantity, opt => opt.MapFrom(o => o.Quantity))
				.ForMember(i => i.Unit, opt => opt.MapFrom(o => o.Unit.Name))
				.ReverseMap();
				
		}
	}
}
