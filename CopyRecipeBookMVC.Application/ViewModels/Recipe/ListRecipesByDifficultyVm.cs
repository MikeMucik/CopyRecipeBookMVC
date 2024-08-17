using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.Recipe
{
	public class ListRecipesByDifficultyVm
	{
		public List<RecipeListForVm> RecipesByDifficulty {  get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int DifficultyId { get; set; } 
		public int Count { get; set; }
	}
}
