using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient
{
    public class IngredientForRecipeVm : IMapFrom<Domain.Model.RecipeIngredient>
    {
        [DisplayName("Nazwa składnika")]
        public string Name { get; set; }
        [DisplayName("Ilość składnika")]
        public decimal Quantity { get; set; }
        [DisplayName("Miara składnika")]
        public string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Model.RecipeIngredient, IngredientForRecipeVm>()
                .ForMember(i => i.Name, opt => opt.MapFrom(o => o.Ingredient.Name))
                .ForMember(i => i.Quantity, opt => opt.MapFrom(o => o.Quantity))
                .ForMember(i => i.Unit, opt => opt.MapFrom(o => o.Unit.Name));
            profile.CreateMap<IngredientForNewRecipeVm, IngredientForRecipeVm>()
                .ForMember(i => i.Name, opt => opt.MapFrom(o => o.NewIngredientName))
                .ForMember(i => i.Unit, opt => opt.MapFrom(o => o.NewIngredientUnit))
                .ForMember(i => i.Quantity, opt => opt.MapFrom(o => o.Quantity))
                .ReverseMap();
		}
    }
}
