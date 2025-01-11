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
			if (recipe == null)
			{
				throw new ArgumentNullException(nameof(recipe), "Nieprawidłowe dane");
			}
			if (_recipeRepo.FindByName(recipe.Name) != null)
			{
				throw new InvalidDataException($"Przepis o Nazwie '{recipe.Name}' już istnieje.");
			}
			if (recipe.Id != 0)
			{
				throw new InvalidOperationException("Numer Id przepisu nie można ustawić ręcznie.");
			}
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
			if (newRecipe == null)
			{
				throw new ArgumentNullException(nameof(newRecipe), "Nieprawidłowe dane");
			}
			if (_recipeRepo.FindByName(newRecipe.Name) != null)
			{
				throw new InvalidOperationException($"Przepis o Nazwie '{newRecipe.Name}' już istnieje.");
			}
			if (newRecipe.Id != 0)
			{
				throw new InvalidOperationException("Numer Id przepisu nie można ustawić ręcznie.");
			}
			var recipeNew = _mapper.Map<Recipe>(newRecipe);
			recipeNew.CategoryId = _categoryService.GetCategoryIdByName(newRecipe.Category);
			recipeNew.DifficultyId = _difficultyService.GetDifficultyIdByName(newRecipe.Difficulty);
			var newTime = new NewRecipeVm
			{
				TimeAmount = newRecipe.TimeAmount,
				TimeUnit = newRecipe.TimeUnit,
			};
			recipeNew.TimeId = _timeService.AddTime(newTime);
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
			var recipe = _recipeRepo.GetRecipeById(id);
			if (recipe != null)
			{
				_recipeRepo.DeleteRecipe(id);
			}
			else
			{
				throw new InvalidOperationException($"Przepis o Id '{id}' nie istnieje.");
			}
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
				throw new InvalidDataException($"Przepis o Id '{id}' nie istnieje.");
			}
			var recipeVm = _mapper.Map<RecipeDetailsVm>(recipe);
			return recipeVm;
		}
		public ListRecipesByCategoryVm GetRecipesByCategory(int pageSize, int pageNumber, int? categoryId, string? categoryName)
		{
			if (categoryId == 0 && string.IsNullOrEmpty(categoryName))
			{
				throw new ArgumentException("Kategoria musi być określona przez ID lub nazwę.");
			}
			var recipes = _recipeRepo.GetRecipesByCategory(categoryId, categoryName)
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
				CategoryName = categoryName,
				Count = recipes.Count
			};
			return recipeList;
		}
		public ListRecipesByDifficultyVm GetRecipesByDifficulty(int pageSize, int pageNumber, int? difficultyId, string? difficultyName = "")
		{
			if (difficultyId == 0 && string.IsNullOrWhiteSpace(difficultyName))
			{
				throw new ArgumentException("Trudność musi być określona przez Id lub nazwę.");
			}
			var recipes = _recipeRepo.GetRecipesByDifficulty(difficultyId, difficultyName)
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
				DifficultyName = difficultyName,
				Count = recipes.Count
			};
			return recipeList;
		}
		public NewRecipeVm GetRecipeToEdit(int id)
		{
			var recipe = _recipeRepo.GetRecipeById(id);
			if (recipe == null)
			{
				return new NewRecipeVm();
			}
			return _mapper.Map<NewRecipeVm>(recipe);
		}
		public void UpdateRecipe(NewRecipeVm recipe)
		{
			var existRecipe = _recipeRepo.RecipeExist(recipe.Id);
			if (!existRecipe)
			{
				throw new KeyNotFoundException($"Przepis o Id '{recipe.Id}' nie istnieje.");
			}
			if (recipe == null)
			{
				throw new ArgumentNullException(nameof(recipe), "Nieprawidłowe dane");
			}
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
			var existRecipe = _recipeRepo.RecipeExist(recipeUpdated.Id);
			if (!existRecipe)
			{
				throw new KeyNotFoundException($"Przepis o Id '{recipeUpdated.Id}' nie istnieje.");
			}

			if (recipeUpdated == null)
			{
				throw new ArgumentNullException(nameof(recipeUpdated), "Nieprawidłowe dane");
			}
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
			List<int>? ingredientIds, List<string>? ingredientsName)
		{
			if ((ingredientIds == null || ingredientIds.Count == 0) && (ingredientsName == null || ingredientsName.Count == 0))
			{
				throw new ArgumentNullException("Liczba składników nie może być pusta.");
			}
			var recipes = _recipeRepo.GetRecipesByIngredients(ingredientIds, ingredientsName)
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
				IngredientNames = ingredientsName,
				Count = recipes.Count
			};
			return recipeList;
		}
		public ListRecipesByTimeVm GetRecipesByTime(int pageSize, int pageNumber, int? timeId, int? timeAmount, string? timeUnit)
		{
			if (timeId == 0 && (string.IsNullOrEmpty(timeUnit) || timeAmount == 0))
			{
				throw new ArgumentException("Wpisz/wybierz czas przygotowania potrawy.");
			}
			var recipes = _recipeRepo.GetRecipesByTime(timeId, timeAmount, timeUnit)
				.OrderBy(r => r.Name)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider)
				.ToList();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipesByTimeVm()
			{
				RecipesByTime = recipesToShow,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				TimeId = timeId,
				TimeAmount = timeAmount,
				TimeUnit = timeUnit,
				Count = recipes.Count()
			};
			return recipeList;
		}		
		public ListRecipesByDetailsVm GetRecipesByDetails(int pageSize, int pageNumber,
			int? categoryId, string? categoryName,
			int? difficultyId, string difficultyName,
			int? timeId, int? timeAmount, string? timeUnit,
			List<int>? ingredientIds, List<string>? ingredientsName)
		{			
			var recipesByCategory = _recipeRepo.GetRecipesByCategory(categoryId, categoryName)				
				.ToList();
			var recipesByDifficulty = _recipeRepo.GetRecipesByDifficulty(difficultyId, difficultyName)				
				.ToList();
			var recipesByTime = _recipeRepo.GetRecipesByTime(timeId, timeAmount, timeUnit)				
				.ToList();
			var recipesByIngredient = _recipeRepo.GetRecipesByIngredients(ingredientIds, ingredientsName)
				.ToList();

			var filteredRecipes = CombineFilters(
				recipesByCategory,
				recipesByDifficulty,
				recipesByTime,
				recipesByIngredient);
			var filteredRecipesMapped = _mapper.Map<List<RecipeListForVm>>(filteredRecipes);
			var recipes = filteredRecipesMapped
					.OrderBy(r => r.Name)
					.ToList();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipesByDetailsVm()
			{
				RecipesByDetails = recipesToShow,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				CategoryId = categoryId,
				CategoryName = categoryName,
				DifficultyId = difficultyId,
				DifficultyName = difficultyName,
				TimeId = timeId,
				TimeAmount = timeAmount,
				TimeUnit = timeUnit,
				IngredientIds = ingredientIds,
				IngredientNames = ingredientsName,
				Count = recipes.Count
			};
			return recipeList;
		}
		private List<Recipe>CombineFilters(
			List<Recipe>? recipesByCategory,
			List<Recipe>? recipesByDifficulty,
			List<Recipe>? recipesByTime,
			List<Recipe>? recipesByIngredient)
		{
			var result = _recipeRepo.GetAllRecipes()				
				.ToList();
			if (recipesByCategory != null && recipesByCategory.Count != 0)
			{
				result = result.Intersect(recipesByCategory).ToList();
			}
			if (recipesByDifficulty != null && recipesByDifficulty.Count !=0)
			{
				result = result.Intersect(recipesByDifficulty).ToList();
			}
			if (recipesByTime != null && recipesByTime.Count != 0)
			{
				result = result.Intersect(recipesByTime).ToList();
			}
			if (recipesByIngredient != null)
			{
				result = result.Intersect(recipesByIngredient).ToList();
			}
			if (!result.Any())
			{
				return new List<Recipe>();
			}
			return result;
		}
	}
}
