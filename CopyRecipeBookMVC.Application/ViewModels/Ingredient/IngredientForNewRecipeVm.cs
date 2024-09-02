using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using FluentValidation;

namespace CopyRecipeBookMVC.Application.ViewModels.Ingredient
{
	public class IngredientForNewRecipeVm:	IMapFrom<Domain.Model.RecipeIngredient>,
											IMapFrom<Domain.Model.Ingredient>,		
											IMapFrom<Domain.Model.Unit>
	{
		
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
	
	// Przyda się jeśli zmienię koncepcje dodawania składników
	//public class IngredientForNewRecipeValidation : AbstractValidator<IngredientForNewRecipeVm>
	//{
 //       public IngredientForNewRecipeValidation()
 //       {
			
	//		RuleFor(i => i.NewIngredientName).MaximumLength(20)
	//			.WithMessage("Nazwa składnika może mieć maksymalnie 20 znaków");
	//		RuleFor(i => i.NewIngredientUnit).MaximumLength(10)
	//			.WithMessage("Miara składnika może mieć maksymalnie 10 znaków");
	//		RuleFor(i => i.Quantity).GreaterThan(0)
	//			.WithMessage("Ilość musi być większa od zera");
 //       }
 //   }
}
