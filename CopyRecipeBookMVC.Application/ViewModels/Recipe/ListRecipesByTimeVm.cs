using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
    public class ListRecipesByTimeVm
    {

        public List<RecipeListForVm> RecipesByTime { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int? TimeId { get; set; }
        public int? TimeAmount { get; set; } 
        public string? TimeUnit { get; set; } 
        public int Count { get; set; }
    }
}
