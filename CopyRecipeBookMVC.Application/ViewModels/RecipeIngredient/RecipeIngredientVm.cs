using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient
{
    public class RecipeIngredientVm
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
    }
}
