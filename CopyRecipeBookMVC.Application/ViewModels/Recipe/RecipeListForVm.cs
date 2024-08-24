using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class RecipeListForVm : IMapFrom<Domain.Model.Recipe>
	{
		public int Id { get; set; }
		[DisplayName("Nazwa przepisu")]
		public string Name { get; set; }
		[DisplayName("Poziom trudności")]
		public string Difficulty { get; set; }
		[DisplayName("Kategoria")]
		public string Category { get; set; }
		[DisplayName("Czas przygotowania dania")]
		public string Time { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Domain.Model.Recipe, RecipeListForVm>()
				.ForMember(n => n.Category, opt => opt.MapFrom(s => s.Category.Name))
				.ForMember(n => n.Difficulty, opt => opt.MapFrom(s => s.Difficulty.Name))
				.ForMember(n => n.Time, opt => opt.MapFrom(s => s.Time.Amount + " " + s.Time.Unit));

		}
	}
}
