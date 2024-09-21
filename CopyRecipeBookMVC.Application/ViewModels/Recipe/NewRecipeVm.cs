using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class NewRecipeVm : IMapFrom<Domain.Model.Recipe>,
								IMapFrom<Domain.Model.Time>
	{
		public int Id { get; set; }
		[DisplayName("Podaj nazwę nowego przepisu")]
		public string Name { get; set; }
		[DisplayName("Wybierz kategorię")]
		public int CategoryId { get; set; }
		[DisplayName("Wybierz poziom trudności")]
		public int DifficultyId { get; set; }


		[DisplayName("Lista czasów")]
		public int? TimeId { get; set; }
		[DisplayName("Ilość nowego czasu")]
		public decimal? TimeAmount { get; set; } //tu mała zmiana z int na decimal
		[DisplayName("Jednostka nowego czasu")]
		public string? TimeUnit { get; set; }
		//[ValidateNever]
		public List<IngredientForNewRecipeVm> Ingredients { get; set; } = new List<IngredientForNewRecipeVm>();
		[DisplayName("Wpisz recepturę przepisu")]
		public string Description { get; set; }
		public int NumberOfIngredients { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<NewRecipeVm, Domain.Model.Recipe>()				
				.ForMember(r => r.CategoryId, opt => opt.MapFrom(i => i.CategoryId))
				.ForMember(r => r.DifficultyId, opt => opt.MapFrom(i => i.DifficultyId))
				.ForMember(r => r.TimeId, opt => opt.MapFrom(t => t.TimeId))				
				.ForMember(r => r.RecipeIngredient, opt => opt.Ignore());
			profile.CreateMap<Domain.Model.Recipe, NewRecipeVm>()
				.ForMember(r=> r.TimeAmount, opt => opt.Ignore())
				.ForMember(r=> r.TimeUnit, opt => opt.Ignore())
				.ForMember(r => r.Ingredients, opt => opt.MapFrom(i => i.RecipeIngredient));
			profile.CreateMap<NewRecipeVm, Domain.Model.Time>()
				.ForMember(r => r.Id, opt => opt.Ignore())// to chyba zbędne ??
				.ForMember(r => r.Amount, opt => opt.MapFrom(t => t.TimeAmount))
				.ForMember(r => r.Unit, opt => opt.MapFrom(t => t.TimeUnit));
			profile.CreateMap<Domain.Model.Time, NewRecipeVm>()
				.ForMember(t => t.TimeId, opt => opt.MapFrom(i => i.Id))
				.ForMember(t => t.TimeAmount, opt => opt.MapFrom(i => i.Amount))
				.ForMember(t => t.TimeUnit, opt => opt.MapFrom(i => i.Unit));
		}
	}
	public class NewRecipeValidation : AbstractValidator<NewRecipeVm>
	{
		public NewRecipeValidation()
		{
			RuleFor(r => r.Id).NotNull();
			RuleFor(r => r.Name)
				.NotNull()
				.WithMessage("Uzupełnij nazwę przepisu")
				.MaximumLength(30)
				.WithMessage("Nazwa przepisu może mieć maksymalnie 30 znaków");
			RuleFor(r => r.TimeId).NotNull()
				.WithMessage("Musisz wybrać czas przygotowania potrawy lub dodać nowy");			
			RuleFor(r => r.Description).NotNull()
				.WithMessage("Uzupełnij recepturę przepisu");
			RuleFor(i => i.NumberOfIngredients)
				.NotNull()
				.WithMessage("Ilość składników nie może być pusta")
				.GreaterThan(0)
				.WithMessage("Ilość składników musi być większa od zera"); 
		}
	}
}
