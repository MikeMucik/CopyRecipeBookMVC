using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CopyRecipeBook.Web.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IDifficultyService _difficultyService;
		private readonly ITimeService _timeService;
		private readonly IIngredientService _ingredientService;
		private readonly IRecipeIngredientService _recipeIngredientService;
		private readonly IUnitService _unitService;		
		private readonly ILogger<RecipeController> _logger;
        public RecipeController(IRecipeService recipeService,
			ICategoryService categoryService,
            IDifficultyService difficultyService,
            ITimeService timeService,
            IIngredientService ingredientService,
			IUnitService unitService,
			IRecipeIngredientService recipeIngredientService,
			ILogger<RecipeController> logger/*, ..*/)
        {
            _recipeService = recipeService;
            _categoryService = categoryService;
            _difficultyService = difficultyService;
            _timeService = timeService;
            _ingredientService = ingredientService;
			_unitService = unitService;
			_recipeIngredientService = recipeIngredientService;
			_logger = logger;
        }
        [HttpGet]
		[Authorize]
        public IActionResult Index(int pageSize=12, int pageNumber=1, string searchString ="")
		{						
			var model = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			return View(model);
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
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
		[Authorize]
		public IActionResult ViewDetails(int id)
		{
			var model = _recipeService.GetRecipe(id);
			return View(model);
		}
		[HttpGet]
		[Authorize]
		public IActionResult ViewByCategory (int pageSize = 12, int pageNumber = 1, int categoryId=0)
		{
			FillViewBags();
			var model = _recipeService.GetRecipesByCategory(pageSize, pageNumber, categoryId);
			return View(model);
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
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
		[Authorize]
		public IActionResult ViewByDifficulty(int pageSize=12, int pageNumber=1, int difficultyId=0)
		{
			FillViewBags();
			var model = _recipeService.GetRecipesByDifficulty(pageSize, pageNumber, difficultyId)
				;
			return View(model);
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
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
		[Authorize(Roles = "Admin, SuperUser, User")]
		public IActionResult AddRecipe()
		{
			FillViewBags();
			return View(new NewRecipeVm());
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		[Authorize(Roles = "Admin, SuperUser, User")]
		public IActionResult AddRecipe(NewRecipeVm model)
		{
			if (!ModelState.IsValid)
			{
				FillViewBags();
				return View(model);
			}
			var existingRecipeId = _recipeService.CheckIfRecipeExists(model.Name);
			//_logger.LogInformation("Jesteś po sprawdzeniu nazwy przepisu");
			if (existingRecipeId != null)
			{						
				return RedirectToAction("EditRecipe", new { id = existingRecipeId });
			}
			_recipeService.AddRecipe(model);
			return RedirectToAction("Index");
		}		
		[HttpGet]
		[Authorize(Roles ="Admin, SuperUser")]
		public IActionResult EditRecipe(int id)
		{
			var recipe = _recipeService.GetRecipeToEdit(id);
			FillViewBags();
			return View(recipe);
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		[Authorize(Roles = "Admin, SuperUser")]
		public IActionResult EditRecipe(NewRecipeVm model)
		{
			if (!ModelState.IsValid)
			{
				FillViewBags();
				return View(model);
			}
			_recipeService.UpdateRecipe(model);
			return RedirectToAction("Index");
		}
		//[HttpPost]
		[AutoValidateAntiforgeryToken]
		[Authorize(Roles = "Admin, SuperUser")]
		// nie powinno być GET
		public IActionResult DeleteRecipe(int id)
		{
			_recipeService.DeleteRecipe(id);
			return RedirectToAction("Index");
		}
		[HttpPost]
		public IActionResult AddIngredient(IngredientForNewRecipeVm model) 
		{			
				var ingredientId = _ingredientService.GetOrAddIngredient(model);
				var unitId = _unitService.GetOrAddUnit(model);
				return Json(new
				{
					success = true,
					ingredientId,
					unitId
				});
		}
		[HttpPost]
		public IActionResult AddTime(NewRecipeVm model)
		{	
			var newTimeId = _timeService.AddTime(model);
			return Json(new { success = true, newTimeId });
		}		
		public void FillViewBags()
		{
			ViewBag.Categories = _categoryService.GetCategorySelectList();			
			ViewBag.Difficulties = _difficultyService.GetDifficultySelectList();			
			ViewBag.Times = _timeService.GetTimeSelectItem();			
			ViewBag.Ingredients = _ingredientService.GetIngredientSelectList();			
			ViewBag.Units = _unitService.GetUnitsForSelectList();			
		}
	}
}
