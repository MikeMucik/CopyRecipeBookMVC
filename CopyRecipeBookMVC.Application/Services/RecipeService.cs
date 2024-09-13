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
			//sprawdzenie czy nazwa jeat już w bazie, musi być jakiś powrót do formularza
			// i komunikat że już jest taki przepis
			//var listOfRecipesBase = _recipeRepo.GetAllRecipes();
   //         foreach (var item in listOfRecipesBase)
   //         {
   //             if (item.Name == recipe.Name)
			//	{
			//		recipe.Id = item.Id;
			//		int idv = recipe.Id;
			//		var editModel = GetRecipeToEdit(idv);
			//		return idv;
					
			//	}
   //         }
            var recipeNew = _mapper.Map<Recipe>(recipe);

			var recipeId = _recipeRepo.AddRecipe(recipeNew);

			foreach (var ingredient in recipe.Ingredients)
			{
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
		public int? CheckIfRecipeExists(string recipeName)
		{
			var existingRecipe = _recipeRepo.GetAllRecipes()
											.FirstOrDefault(r => r.Name == recipeName);

			return existingRecipe?.Id; // Zwróć Id przepisu, jeśli istnieje, w przeciwnym razie null
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

		public NewRecipeVm GetRecipeToEdit(int id)
		{
			var recipe = _recipeRepo.GetRecipeById(id);
			var recipeVm = _mapper.Map<NewRecipeVm>(recipe);
			return recipeVm;
			
		}

		public int UpdaterRecipe(Recipe recipe)
		{
			throw new NotImplementedException();
		}

	}
}
