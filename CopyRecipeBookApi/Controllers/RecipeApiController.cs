using System.Drawing.Printing;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CopyRecipeBookApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class RecipeApiController : ControllerBase
	{
		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IDifficultyService _difficultyService;
		private readonly ITimeService _timeService;
		private readonly IIngredientService _ingredientService;
		private readonly IRecipeIngredientService _recipeIngredientService;
		private readonly IUnitService _unitService;
		private readonly ILogger<RecipeApiController> _logger;
		public RecipeApiController(IRecipeService recipeService,
			ICategoryService categoryService,
			IDifficultyService difficultyService,
			ITimeService timeService,
			IIngredientService ingredientService,
			IUnitService unitService,
			IRecipeIngredientService recipeIngredientService,
			ILogger<RecipeApiController> logger/*, ..*/)
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

		[HttpGet(Name = "Index")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<ListRecipeForListVm> Index(int pageSize = 12, int pageNumber = 1, string searchString = "")
		{
			searchString ??= string.Empty;
			var model = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			return Ok(model);
		}

		// GET api/<RecipeApiController>/5
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<RecipeDetailsVm> ViewDetails(int id)
		{
			var model = _recipeService.GetRecipe(id);
			if (model == null)
			{
				return NotFound();
			}
			return Ok(model);
		}

		// POST api/<RecipeApiController>
		[HttpPost]
		public IActionResult AddRecipeApi([FromBody] NewRecipeDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingRecipe = _recipeService.CheckNameForRecipe(model.Name);
			if (existingRecipe)
			{
				return Conflict();//Status 409
			}

			var newRecipeId = _recipeService.AddRecipeApi(model);
			return StatusCode(201, new { RecipeId = newRecipeId });			
		}
		

		// PUT api/<RecipeApiController>/5
		[HttpPut("{id}")]
		public IActionResult EditRecipeApi(int id, [FromBody] NewRecipeDTO model) 
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (id != model.Id)
			{
				return BadRequest("ID in the URL does not match ID in the body.");
			}

			var result = _recipeService.UpdateRecipeApi(model);
			if (!result)
			{
				return NotFound();
			}
			return StatusCode (200);
				
		}		

		// DELETE api/<RecipeApiController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			_recipeService.DeleteRecipe(id);
			return StatusCode(204);
		}
	}
}
