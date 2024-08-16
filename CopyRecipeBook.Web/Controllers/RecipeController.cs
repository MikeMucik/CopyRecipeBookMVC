using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using Microsoft.AspNetCore.Mvc;

namespace CopyRecipeBook.Web.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
			_recipeService = recipeService;
        }
		[HttpGet]
        public IActionResult Index()
		{
			//widok
			//tabela przepisów
			//filtrowanie 
			//dane
			//filtry do serwisu
			//przyg
			//dane w formie
			var model = _recipeService.GetAllRecipesForList(2, 1, "");
			return View(model);
		}
		[HttpPost]
		public IActionResult Index(int pageSize, int? pageNumber, string searchString)
		{
			if (!pageNumber.HasValue)
			{
				pageNumber = 1;
			}
			if (searchString is null)
			{
				searchString = string.Empty; 
			}
			var model = _recipeService.GetAllRecipesForList(pageSize, pageNumber.Value, searchString);
			return View(model);
		}
		
		[HttpGet]
		public IActionResult ViewDetails(int id)
		{
			var model = _recipeService.GetRecipe(id);
			return View(model);
		}


		[HttpGet]
		public IActionResult ViewByCategory ()
		{
			var model = _recipeService.GetRecipesByCategory(1, 1, 0);
			return View(model);
		}

		[HttpPost] 
		public IActionResult ViewByCategory (int pageSize, int? pageNumber, int categoryId)
		{
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            
            var model = _recipeService.GetRecipesByCategory(pageSize, pageNumber.Value, categoryId);
            return View(model);
        }

		[HttpGet]
		public IActionResult AddRecipe()
		{
			return View();
		}
		[HttpPost]
		public IActionResult AddRecipe(NewRecipeVm model)
		{
			var id = _recipeService.AddRecipe(model);
			return View();
		}
	}
}
