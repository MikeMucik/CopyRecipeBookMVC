using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Category;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface ICategoryService
	{
		ListCategoryForListVm GetListCategoryForList();
		List<SelectListItem> GetCategoryForSelectList();
		int GetCategoryIdByName(string name);
	}
}
