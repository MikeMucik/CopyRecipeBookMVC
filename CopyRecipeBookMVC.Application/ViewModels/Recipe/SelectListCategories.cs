using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class SelectListCategories : IMapFrom<Domain.Model.Category> //do wywalenia po spr
	{
		public string Text { get; set; }
		public string Value { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Domain.Model.Category, SelectListCategories>()
				.ForMember(c => c.Text, opt => opt.MapFrom(i => i.Name))
				.ForMember(c => c.Value, opt => opt.MapFrom(i => i.Name));

		}
	}
}