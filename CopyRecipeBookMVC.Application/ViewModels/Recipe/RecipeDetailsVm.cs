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
    public class RecipeDetailsVm : IMapFrom<Domain.Model.Recipe>
    {
        public int Id { get; set; }
        [DisplayName("Nazwa przepisu")]
        public string Name { get; set; }
        [DisplayName("Kategoria")]
        public string Category { get; set; }
        [DisplayName("Poziom trudności")]
        public string Difficulty { get; set; }
        [DisplayName("Czas przygotowania przepisu")]
        public string Time { get; set; }

        public List<IngredientForRecipeVm> Ingredients { get; set; }
        [DisplayName("Receptura przygotowania przepisu")]
        public string Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Model.Recipe, RecipeDetailsVm>()
                .ForMember(d => d.Category, opt => opt.MapFrom(q => q.Category.Name))
                .ForMember(d => d.Difficulty, opt => opt.MapFrom(q => q.Difficulty.Name))
                .ForMember(d => d.Time, opt => opt.MapFrom(q => q.Time.Amount + " " + q.Time.Unit))
                .ForMember(d => d.Ingredients, opt => opt.MapFrom(q => q.RecipeIngredient));
        }
    }
}
