using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using FluentValidation;

namespace CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient
{
	public class IngredientForNewRecipeVm:	IMapFrom<Domain.Model.RecipeIngredient>,
											IMapFrom<Domain.Model.Ingredient>,		
											IMapFrom<Domain.Model.Unit>
	{		
		public int IngredientName { get; set; }
		public decimal Quantity { get; set; }		
		public int IngredientUnit { get; set; }
		public string? NewIngredientName { get; set; }		//
		public string? NewIngredientUnit { get; set; }	//
		public void Mapping(Profile profile)
		{
			profile.CreateMap<IngredientForNewRecipeVm, Domain.Model.RecipeIngredient>()
				.ForMember(ri => ri.IngredientId, opt => opt.MapFrom(i => i.IngredientName))
				.ForMember(ri => ri.UnitId, opt => opt.MapFrom(i => i.IngredientUnit))
				.ForMember(ri => ri.Quantity, opt => opt.MapFrom(i => i.Quantity))
				.ReverseMap();
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
	public class IngredientForNewRecipeValidation : AbstractValidator<IngredientForNewRecipeVm>
	{
		public IngredientForNewRecipeValidation()
		{
			RuleFor(i => i.NewIngredientName)
				.MaximumLength(20)
				.WithMessage("Nazwa składnika może mieć maksymalnie 20 znaków")
				.When(i => i.IngredientName == 0)
				.Empty()
				.WithMessage("Jeśli wybrano składnik z listy to nazwa musi być pusta")
				.When(i => i.IngredientName > 0)
				.NotEmpty()
				.WithMessage("Proszę wpisać składnik lub wybrać z listy")
				.When(i => i.IngredientName == 0);
			RuleFor(i=> i)
				.Must(i =>  !(i.IngredientName > 0 && !string.IsNullOrEmpty(i.NewIngredientName)))
				.WithMessage("Wybierz składnik z listy lub wpisz nowy, ale nie oba jednocześnie");		
			RuleFor(i => i.IngredientName)
				.GreaterThan(0)
				.When(i => i.NewIngredientName == "")
				.WithMessage("Nieprawidłowa wartość");	
			RuleFor(i => i.NewIngredientUnit).MaximumLength(10)
				.WithMessage("Miara składnika może mieć maksymalnie 10 znaków")
				.When(i=>i.IngredientUnit == 0)
				.Empty()
				.WithMessage("Jeśli wybrano miarę z listy to pole musi być puste")
				.When(i=>i.IngredientUnit > 0)
				.NotEmpty()
				.WithMessage("Proszę wpisać miarę lub wybrać z listy");
			RuleFor(i => i)
				.Must(i => !(i.IngredientUnit > 0 && !string.IsNullOrEmpty(i.NewIngredientUnit)))
				.WithMessage("Wybierz miarę z listy lub wpisz nową, ale nie obie jednocześnie");
			RuleFor(i => i.IngredientUnit)
				.GreaterThan(0)
				.When(i => i.NewIngredientUnit == "")
				.WithMessage("Nieparwidłowa wartość");
			RuleFor(i => i.Quantity).GreaterThan(0)
				.WithMessage("Ilość musi być większa od zera");			
		}
	}
}
