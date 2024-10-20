using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.Time;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface ITimeService
	{
		int AddTime (NewRecipeVm time);
		//Time GetTime (int id);
		ListTimeForListVm GetListTimeForList ();
		List<SelectListItem> GetTimeSelectItem();

	}
}
