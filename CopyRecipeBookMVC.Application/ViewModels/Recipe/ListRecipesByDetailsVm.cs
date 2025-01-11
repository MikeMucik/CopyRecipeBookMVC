using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
    public class ListRecipesByDetailsVm
    {
        public List<RecipeListForVm> RecipesByDetails { get; set; } = new List<RecipeListForVm>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } 
        public int? DifficultyId { get; set; }
        public string? DifficultyName { get; set; }
        public int? TimeId { get; set; }
        public int? TimeAmount { get; set; }
        public string? TimeUnit { get; set; }
        public List<int>? IngredientIds { get; set; } = new List<int>();// może trzeba zainicjolizaować
        public List<string>? IngredientNames { get; set; }
        public int Count { get; set; }
    }
}
