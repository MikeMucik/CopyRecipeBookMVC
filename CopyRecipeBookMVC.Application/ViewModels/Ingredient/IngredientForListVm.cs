using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Ingredient
{
   public class IngredientForListVm :IMapFrom<Domain.Model.Ingredient>
    {
        public int Id { get; set; }
        public string Name { get; set; }
      
        public void Mapping(Profile profile )
        {
            profile.CreateMap<Domain.Model.Ingredient, IngredientForListVm>();
        }

    }
}
