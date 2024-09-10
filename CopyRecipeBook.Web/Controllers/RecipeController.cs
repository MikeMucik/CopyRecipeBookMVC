using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBook.Web.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IDifficultyService _difficultyService;
		private readonly ITimeService _timeService;
		private readonly IIngredientService _ingredientService;
		private readonly IUnitService _unitService;
        public RecipeController(IRecipeService recipeService,
			ICategoryService categoryService,
            IDifficultyService difficultyService,
            ITimeService timeService,
            IIngredientService ingredientService,
			IUnitService unitService)
        {
            _recipeService = recipeService;
            _categoryService = categoryService;
            _difficultyService = difficultyService;
            _timeService = timeService;
            _ingredientService = ingredientService;
			_unitService = unitService;
        }
        [HttpGet]
        public IActionResult Index()
		{			
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
			FillViewBags();
			return View(new NewRecipeVm());
		}

		[HttpPost]
		public IActionResult AddRecipe(NewRecipeVm model)
		{
			if (!ModelState.IsValid)
			{
				FillViewBags();
				return View(model);
			}
				_recipeService.AddRecipe(model);
			return RedirectToAction("Index");
		}

		//Przyda się jeśli zmienię koncepcję dodawania składników

		

		[HttpPost]
		public IActionResult AddIngredient(IngredientForNewRecipeVm model) //co to jtest to frombody
		{
			
				// Sprawdź, czy składnik i jednostka już istnieją
				var ingredientId = _ingredientService.GetOrAddIngredient(model);
				var unitId = _unitService.GetOrAddUnit(model);

				return Json(new
				{
					success = true,
					ingredientId = ingredientId,
					unitId = unitId
				});
		}

		[HttpPost]
		public IActionResult AddTime(NewRecipeVm model)
		{	
			var newTimeId = _timeService.AddTime(model);
			return Json(new { success = true, newTimeId = newTimeId });
		}

		//Czy to nie powinno być w innym miejscu ??

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

            var timeListVm = _timeService.GetListTimeForList();
            ViewBag.Times = timeListVm.Times.Select(tim => new SelectListItem
            {
                Value = tim.Id.ToString(),
                Text = tim.Amount.ToString() + " " + tim.Unit
            });

            var ingredientListVm = _ingredientService.GetListIngredientForList();
            ViewBag.Ingredients = ingredientListVm.Ingredients.Select(ing => new SelectListItem
            {
                Value = ing.Id.ToString(),
                Text = ing.Name, 
            });

			var unitListVm = _unitService.GetAllUnitsForList();
			ViewBag.Units = unitListVm.Units.Select(uni => new SelectListItem
			{
				Value = uni.Id.ToString(),
				Text = uni.Name,
			});
		}
	}
}
