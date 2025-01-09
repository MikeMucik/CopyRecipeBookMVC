using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using FluentValidation;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class NewRecipeDTO : IMapFrom<Domain.Model.Recipe>,
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
		public void Mapping(Profile profile)
		{
			profile.CreateMap<NewRecipeDTO, Domain.Model.Recipe>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) 
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Category, opt => opt.Ignore()) 
				.ForMember(dest => dest.Difficulty, opt => opt.Ignore())
				.ForMember(dest => dest.TimeId, opt => opt.Ignore())
				.ForMember(dest => dest.RecipeIngredient, opt => opt.Ignore());				
		}
	}
	public class NewRecipeDTOValidation : AbstractValidator<NewRecipeDTO>
	{
		public NewRecipeDTOValidation()
		{
			RuleFor(r => r.Id)
				.NotNull()
				.GreaterThan(0);
			RuleFor(r => r.Name)
				.NotNull()
				.WithMessage("Uzupełnij nazwę przepisu")
				.MaximumLength(30)
				.WithMessage("Nazwa przepisu może mieć maksymalnie 30 znaków");		
			RuleFor(r => r.TimeAmount)				
				.InclusiveBetween(1, 59)
				.WithMessage("Ilośc czasu musi większa od zera i mniejsza od 60");				
			RuleFor(r => r.TimeUnit)
				.NotEmpty()
				.WithMessage("Jednostka czasu nie może być pusta");				
			RuleFor(r => r.Description)
				.NotNull()
				.WithMessage("Uzupełnij recepturę przepisu");			
		}
	}
}
