using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Difficulty;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface IDifficultyService
	{
		ListDifficultyForListVm GetListDifficultyForList();
		List<SelectListItem> GetDifficultySelectList();
		int GetDifficultyIdByName(string name);
	}
}

