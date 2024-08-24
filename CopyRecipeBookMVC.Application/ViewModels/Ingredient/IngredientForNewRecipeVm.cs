using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Ingredient
{
	public class IngredientForNewRecipeVm:	IMapFrom<Domain.Model.RecipeIngredient>,
											IMapFrom<Domain.Model.Ingredient>,		
											IMapFrom<Domain.Model.Unit>
	{
		public int NumberOfIngredients { get; set; }//
		public int Name { get; set; }
		public decimal Quantity { get; set; }
		public int Unit { get; set; }

		public string NewIngredientName { get; set; }
		
		public string NewIngredientUnit { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<IngredientForNewRecipeVm, Domain.Model.RecipeIngredient>()
				.ForMember(ri => ri.IngredientId, opt => opt.MapFrom(i => i.Name))
				.ForMember(ri => ri.UnitId, opt => opt.MapFrom(i => i.Unit))
				.ForMember(ri => ri.Quantity, opt => opt.MapFrom(i => i.Quantity));
			// Mapowanie do Ingredient
			profile.CreateMap<IngredientForNewRecipeVm, Domain.Model.Ingredient>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NewIngredientName));
			// Mapowanie do Unit
			profile.CreateMap<IngredientForNewRecipeVm, Domain.Model.Unit>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NewIngredientUnit));		
		}
	}
}
