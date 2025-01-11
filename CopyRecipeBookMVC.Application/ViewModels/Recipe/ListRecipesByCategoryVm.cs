using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
    public class ListRecipesByCategoryVm
    {
        public List<RecipeListForVm> RecipesByCategory { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int? CategoryId { get; set; } 
        public string? CategoryName { get; set; } //dla WebApi
        public int Count { get; set; }        
    }
}
