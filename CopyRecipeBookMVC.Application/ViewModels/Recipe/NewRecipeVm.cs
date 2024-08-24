using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;

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
		public int? TimeAmount { get; set; }
		[DisplayName("Jednostka nowego czasu")]
		public string? TimeUnit { get; set; }
		public List<IngredientForNewRecipeVm> Ingredients { get; set; } = new List<IngredientForNewRecipeVm>();
		[DisplayName("Wpisz recepturę przepisu")]
		public string Description { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<NewRecipeVm, Domain.Model.Recipe>()
				.ForMember(r => r.CategoryId, opt => opt.MapFrom(i => i.CategoryId))
				.ForMember(r => r.DifficultyId, opt => opt.MapFrom(i => i.DifficultyId))

				.ForMember(r => r.TimeId, opt => opt.MapFrom(t => t.TimeId))
				

				.ForMember(r => r.RecipeIngredient, opt => opt.Ignore())
				;
			profile.CreateMap<NewRecipeVm, Domain.Model.Time>() 
				.ForMember(r=>r.Id, opt => opt.Ignore())
				.ForMember(r => r.Amount, opt => opt.MapFrom(t => t.TimeAmount))
				.ForMember(r=> r.Unit, opt => opt.MapFrom(t => t.TimeUnit));//
		}
	}
}
