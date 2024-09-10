using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly IRecipeRepository _recipeRepo;
		private readonly IIngredientRepository _ingredientRepo;
		private readonly IMapper _mapper;
		private readonly ICategoryRepository _categoryRepo;
		private readonly IIngredientService _ingredientService;
		private readonly IUnitService _unitService;
		private readonly ITimeService _timeService;

		public RecipeService(IRecipeRepository recipeRepo,
			IIngredientRepository ingredientRepo,
			ICategoryRepository categoryRepo,
			IMapper mapper,
			IIngredientService ingredientService,
			IUnitService unitService,
			ITimeService timeService)
		{
			_recipeRepo = recipeRepo;
			_ingredientRepo = ingredientRepo;
			_mapper = mapper;
			_categoryRepo = categoryRepo;
			_ingredientService = ingredientService;
			_unitService = unitService;
			_timeService = timeService;
		}
		public int AddRecipe(NewRecipeVm recipe)
		{
			
			var recipeNew = _mapper.Map<Recipe>(recipe);

			//recipeNew.TimeId = recipe.TimeId.Value;
			var recipeId = _recipeRepo.AddRecipe(recipeNew);

			foreach (var ingredient in recipe.Ingredients)
			{
				//int ingredientId = GetOrAddIngredient(ingredient);

				//int unitId = GetOrAddUnit(ingredient);

				var recipeIngredient = new RecipeIngredient
				{
					RecipeId = recipeId,
					IngredientId = ingredient.Name,
					UnitId = ingredient.Unit,
					Quantity = ingredient.Quantity
				};
				_ingredientService.AddCompleteIngredients(recipeIngredient);
			}

			return recipeId;
		}

		//private int GetOrAddTime(NewRecipeVm recipe)
		//{

		//	if (recipe.TimeId == null )
		//	{
		//		var timeId = _timeService.AddTime(new NewRecipeVm
		//		{
		//			TimeAmount = recipe.TimeAmount,
		//			TimeUnit = recipe.TimeUnit
		//		});
		//		//recipe.TimeSelected = 1;//
		//		return timeId;
		//	}
		//	else
		//	{
		//		//recipe.TimeSelected = 1;//
		//		return recipe.TimeId.Value;
		//	}

		//}

		//public int GetOrAddIngredient(IngredientForNewRecipeVm ingredient)
		//{
		//	//sprawdzenie czy nazwa składnika istnieje w bazie
		//	var listOfIngredient = _ingredientService.GetListIngredientForList();
		//	foreach (var ing in listOfIngredient.Ingredients)

		//	{
		//		if (ing.Name == ingredient.NewIngredientName)
		//		{
		//			return ing.Id;
		//		}

		//	}
		//	if (string.IsNullOrEmpty(ingredient.NewIngredientName))
		//	{
		//		return ingredient.Name; // Zakładając, że ingredient.Name to ID istniejącego składnika
		//	}
		//	else
		//	{
		//		return _ingredientService.AddIngredient(new IngredientForNewRecipeVm { NewIngredientName = ingredient.NewIngredientName });
		//	}
		//}

		public int GetOrAddUnit(IngredientForNewRecipeVm ingredient)
		{
			var listOfUnit = _unitService.GetAllUnitsForList();
            foreach (var unit in listOfUnit.Units)
            {
                if (unit.Name == ingredient.NewIngredientUnit)
				{
					return unit.Id;
				}
            }
            if (string.IsNullOrEmpty(ingredient.NewIngredientUnit))
			{
				return ingredient.Unit; // Zakładając, że ingredient.Unit to ID istniejącej jednostki
			}
			else
			{
				return _unitService.AddUnit(new IngredientForNewRecipeVm { NewIngredientUnit = ingredient.NewIngredientUnit });
			}
		}

		public void DeleteRecipe(int id)
		{
			throw new NotImplementedException();
		}

		public ListRecipeForListVm GetAllRecipesForList(int pageSize, int pageNumber, string searchString)
		{
			var recipes = _recipeRepo.GetAllRecipes()
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
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r => categoryId == r.Category.Id)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider).ToList();

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
				Count = recipes.Count,
			};

			return recipeList;
		}
		public ListRecipesByDifficultyVm GetRecipesByDifficulty(int pageSize, int pageNumber, int difficultyId)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r => difficultyId == r.Difficulty.Id)
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

		public int UpdaterRecipe(Recipe recipe)
		{
			throw new NotImplementedException();
		}

	}
}
