using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBook.Web.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IDifficultyService _difficultyService;
        public RecipeController(IRecipeService recipeService,
			ICategoryService categoryService
,
			IDifficultyService difficultyService)
		{
			_recipeService = recipeService;
			_categoryService = categoryService;
			_difficultyService = difficultyService;
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
			FillViewBags();
			var model = _recipeService.GetRecipesByCategory(1, 1, 0)
				;
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
			FillViewBags();
            return View(model);
        }

		[HttpGet]
		public IActionResult ViewByDifficulty()
		{
			FillViewBags();
			var model = _recipeService.GetRecipesByDifficulty(1, 1, 0)
				;
			return View(model);
		}

		[HttpPost]
		public IActionResult ViewByDifficulty(int pageSize, int? pageNumber, int difficultyId)
		{
			if (!pageNumber.HasValue)
			{
				pageNumber = 1;
			}
			
			var model = _recipeService.GetRecipesByDifficulty(pageSize, pageNumber.Value, difficultyId);
			FillViewBags();
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
		public void FillViewBags()
		{
			var categoryListVm = _categoryService.GetListCategoryForList();
			ViewBag.Categories = categoryListVm.Categories.Select(cat => new SelectListItem
			{
				Value = cat.Id.ToString(),
				Text = cat.Name,	
			}).ToList();
			var difficultyListVm = _difficultyService.GetListDifficultyForList();
			ViewBag.Difficulties = difficultyListVm.Difficulties.Select(cat => new SelectListItem
			{
				Value = cat.Id.ToString(),
				Text = cat.Name,
			}).ToList();
		}
	}
}
