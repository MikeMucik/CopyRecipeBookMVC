using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;


namespace CopyRecipeBookMVC.Application.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly IRecipeRepository _recipeRepo;
		private readonly IMapper _mapper;
		private readonly IRecipeIngredientService _recipeIngredientService;
		private readonly ITimeService _timeService;
		private readonly IIngredientService _ingredientService;
		private readonly IUnitService _unitService;
		private readonly ICategoryService _categoryService;
		private readonly IDifficultyService _difficultyService;
		public RecipeService(
			IRecipeRepository recipeRepo,
			IMapper mapper,
			IRecipeIngredientService recipeIngredientService,
			ITimeService timeService,
			IIngredientService ingredientService,
			IUnitService unitService,
			ICategoryService categoryService,
			IDifficultyService difficultyService)
		{
			_recipeRepo = recipeRepo;
			_mapper = mapper;
			_recipeIngredientService = recipeIngredientService;
			_timeService = timeService;
			_ingredientService = ingredientService;
			_unitService = unitService;
			_categoryService = categoryService;
			_difficultyService = difficultyService;
		}
		public RecipeService(
			IRecipeRepository recipeRepo,
			IMapper mapper,
			IRecipeIngredientService recipeIngredientService)
		{
			_recipeRepo = recipeRepo;
			_mapper = mapper;
			_recipeIngredientService = recipeIngredientService;		
		}
		public int AddRecipe(NewRecipeVm recipe)
		{
			var recipeNew = _mapper.Map<Recipe>(recipe);
			var recipeId = _recipeRepo.AddRecipe(recipeNew);
			foreach (var ingredient in recipe.Ingredients)
			{
				var recipeIngredient = new RecipeIngredient
				{
					RecipeId = recipeId,
					IngredientId = ingredient.IngredientName,
					UnitId = ingredient.IngredientUnit,
					Quantity = ingredient.Quantity
				};
				_recipeIngredientService.AddCompleteIngredients(recipeIngredient);
			}
			return recipeId;
		}
		public int AddRecipeApi(NewRecipeDTO newRecipe)
		{
			var recipeNew = new Recipe();

			recipeNew.Id = newRecipe.Id;
			recipeNew.Name = newRecipe.Name;
			recipeNew.CategoryId = _categoryService.GetCategoryIdByName(newRecipe.Category);
			recipeNew.DifficultyId = _difficultyService.GetDifficultyIdByName(newRecipe.Difficulty);
			var newTime = new NewRecipeVm
			{
				TimeAmount = newRecipe.TimeAmount,
				TimeUnit = newRecipe.TimeUnit,
			};
			recipeNew.TimeId = _timeService.AddTime(newTime);
			recipeNew.Description = newRecipe.Description;
			recipeNew.RecipeIngredient.Clear();
			foreach (var ingredient in newRecipe.Ingredients)
			{
				var newIngredient = _mapper.Map<IngredientForNewRecipeVm>(ingredient);
				int ingredientId = _ingredientService.GetOrAddIngredient(newIngredient);

				int unitId = _unitService.GetOrAddUnit(newIngredient);

				var recipeIngredient = new RecipeIngredient
				{
					RecipeId = recipeNew.Id,
					IngredientId = ingredientId,
					UnitId = unitId,
					Quantity = ingredient.Quantity
				};
				recipeNew.RecipeIngredient.Add(recipeIngredient);
			}
			_recipeRepo.AddRecipe(recipeNew);			
			return recipeNew.Id;
		}
		public bool CheckNameForRecipe(string name)
		{
			var exist = _recipeRepo.FindByName(name);
			if (exist == null)
			{
				return false;
			}
			else { return true; }
		}
		public int? TryAddRecipe(NewRecipeVm model)
		{
			var existingRecipe = _recipeRepo.FindByName(model.Name);
			if (existingRecipe != null)
				return existingRecipe.Id;
			return null;
		}
		public void DeleteRecipe(int id)
		{
			_recipeRepo.DeleteRecipe(id);
		}
		public ListRecipeForListVm GetAllRecipesForList(int pageSize, int pageNumber, string searchString)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.OrderBy(x => x.Name)
				.Where(r => r.Name.StartsWith(searchString))
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider)
				.ToList();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipeForListVm()
			{
				Recipes = recipesToShow,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				SearchString = searchString,
				Count = recipes.Count
			};
			return recipeList;
		}
		public RecipeDetailsVm GetRecipe(int id)
		{
			var recipe = _recipeRepo.GetRecipeById(id);
			if (recipe == null)
			{
				throw new Exception("Recipe is null");
			}
			var recipeVm = _mapper.Map<RecipeDetailsVm>(recipe);
			return recipeVm;
		}

		public ListRecipesByCategoryVm GetRecipesByCategory(int pageSize, int pageNumber, int categoryId)
		{
			var recipes = _recipeRepo.GetRecipesByCategory(categoryId)
				.OrderBy(r => r.Name)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider)
				.ToList();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipesByCategoryVm()
			{
				RecipesByCategory = recipesToShow,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				CategoryId = categoryId,
				Count = recipes.Count()
			};
			return recipeList;
		}
		public ListRecipesByDifficultyVm GetRecipesByDifficulty(int pageSize, int pageNumber, int difficultyId)
		{
			var recipes = _recipeRepo.GetRecipesByDifficulty(difficultyId)
				.OrderBy(x => x.Name)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider).ToList();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipesByDifficultyVm()
			{
				RecipesByDifficulty = recipesToShow,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				DifficultyId = difficultyId,
				Count = recipes.Count
			};
			return recipeList;
		}
		public NewRecipeVm GetRecipeToEdit(int id)
		{
			try
			{
				var recipe = _recipeRepo.GetRecipeById(id);
				return _mapper.Map<NewRecipeVm>(recipe);
			}
			catch (InvalidOperationException)
			{
				return new NewRecipeVm();
			}
		}

		public void UpdateRecipe(NewRecipeVm recipe)
		{
			_recipeIngredientService.DeleteCompleteIngredients(recipe.Id);
			Recipe editRecipe = _mapper.Map<Recipe>(recipe);
			_recipeRepo.UpdateRecipe(editRecipe);
			foreach (var ingredient in recipe.Ingredients)
			{
				var recipeIngredient = new RecipeIngredient
				{
					RecipeId = recipe.Id,
					IngredientId = ingredient.IngredientName,
					UnitId = ingredient.IngredientUnit,
					Quantity = ingredient.Quantity
				};
				_recipeIngredientService.AddCompleteIngredients(recipeIngredient);
			}
		}
		public bool UpdateRecipeApi(NewRecipeDTO recipeUpdated)
		{			
			var recipeNew = new Recipe();
			
			recipeNew.Id = recipeUpdated.Id;
			recipeNew.Name = recipeUpdated.Name;
			recipeNew.CategoryId = _categoryService.GetCategoryIdByName(recipeUpdated.Category);
			recipeNew.DifficultyId = _difficultyService.GetDifficultyIdByName(recipeUpdated.Difficulty);
			var newTime = new NewRecipeVm
			{
				TimeAmount = recipeUpdated.TimeAmount,
				TimeUnit = recipeUpdated.TimeUnit,
			};
				recipeNew.TimeId = _timeService.AddTime(newTime);			
			recipeNew.Description = recipeUpdated.Description;
			recipeNew.RecipeIngredient.Clear();
			foreach (var ingredient in recipeUpdated.Ingredients)
			{
				var newIngredient = _mapper.Map<IngredientForNewRecipeVm>(ingredient);
				int ingredientId = _ingredientService.GetOrAddIngredient(newIngredient);
				
				int unitId = _unitService.GetOrAddUnit(newIngredient);

				var recipeIngredient = new RecipeIngredient
				{
					RecipeId = recipeNew.Id,
					IngredientId = ingredientId,
					UnitId = unitId,
					Quantity = ingredient.Quantity
				};				
				recipeNew.RecipeIngredient.Add(recipeIngredient);
			}	
			_recipeRepo.UpdateRecipe(recipeNew);
			return true;
		}
		public ListRecipesByIngredientsVm GetRecipesByIngredients(int pageSize, int pageNumber,
			List<int> ingredientIds)
		{
			var recipes = _recipeRepo.GetRecipesByIngredients(ingredientIds)
				.OrderBy(x => x.Name)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider)
				.ToList();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipesByIngredientsVm()
			{
				RecipesByIngredients = recipesToShow,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				IngredientIds = ingredientIds,
				Count = recipes.Count
			};
			return recipeList;
		}
	}
}
