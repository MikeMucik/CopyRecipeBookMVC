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

		[HttpGet("Category")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<ListRecipeForListVm> GetByCategory(int pageSize = 12, int pageNumber = 1, int categoryId = 0, string categoryName = "")
		{
			try
			{
				var model = _recipeService.GetRecipesByCategory(pageSize, pageNumber, categoryId, categoryName);
				return Ok(model);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}

		}

		[HttpGet("FindByDetails")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<ListRecipeForListVm> GetByDetails(int pageSize = 12, int pageNumber = 1,
			int categoryId = 0, string categoryName = "",
			int difficultyId = 0, string difficultyName = "",
			int timeId = 0, int timeAmount = 0, string timeUnit = "",
			[FromQuery] List<int>? ints = null,
			[FromQuery] List<string>? strings = null)
		{
			try
			{
				var model = _recipeService.GetRecipesByDetails(pageSize, pageNumber,
					categoryId, categoryName,
					difficultyId, difficultyName,
					timeId , timeAmount , timeUnit,
					ints, strings);
				return Ok(model);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}

		}
		[HttpGet("Difficulty")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<ListRecipeForListVm> GetByDifficulty(int pageSize = 12, int pageNumber = 1, int difficultyId = 0, string difficultyName = "")
		{
			var model = _recipeService.GetRecipesByDifficulty(pageSize, pageNumber, difficultyId, difficultyName);
			return Ok(model);
		}
        [HttpGet("Time")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ListRecipeForListVm> GetByTime(int pageSize = 12, int pageNumber = 1, int timeId = 0, int timeAmount = 0, string timeUnit = "")
        {
            var model = _recipeService.GetRecipesByTime(pageSize, pageNumber, timeId, timeAmount, timeUnit);
            return Ok(model);
        }
        [HttpGet("Ingredients")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<ListRecipeForListVm> GetByIngredients(
			int pageSize = 12,
			int pageNumber = 1,
			[FromQuery] List<int>? ints = null,
			[FromQuery] List<string>? strings = null)
		{
			ints ??= new List<int>();
			strings ??= new List<string>();
			var model = _recipeService.GetRecipesByIngredients(pageSize, pageNumber, ints, strings);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AddRecipeApi([FromBody] NewRecipeDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var recipeId = _recipeService.AddRecipeApi(model);
				return CreatedAtAction(nameof(ViewDetails), new { id = recipeId }, recipeId);				
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(new { message = ex.Message });
			}			
		}
		// PUT api/<RecipeApiController>/5
		[HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
			return StatusCode(200);
		}
		// DELETE api/<RecipeApiController>/5
		[HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Delete(int id)
		{
			_recipeService.DeleteRecipe(id);
			return StatusCode(204);
		}
	}
}
