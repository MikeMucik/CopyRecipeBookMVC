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
			var existingRecipe = _recipeRepo.GetAllRecipes().FirstOrDefault
				//(r => string.Equals(r.Name, recipeName, StringComparison.OrdinalIgnoreCase));
				(r => r.Name.ToLower() == recipeName.ToLower());			
			return existingRecipe?.Id; // Zwróć Id przepisu, jeśli istnieje, w przeciwnym razie null
		}
		public void DeleteRecipe(int id)
		{
			_recipeRepo.DeleteRecipe(id);
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
			Recipe recipe = _recipeRepo.GetRecipeById(id);
			if (recipe == null)
			{
				return new NewRecipeVm();
			}
			else {
			return _mapper.Map<NewRecipeVm>(recipe);
			 }			
		}
		public void UpdateRecipe(NewRecipeVm recipe)
		{
			_ingredientService.DeleteCompleteIngredients(recipe.Id);
			Recipe editRecipe = _mapper.Map<Recipe>(recipe);
			_recipeRepo.UpdateRecipe(editRecipe);
			//usuń rekordy z tabeli składników i dodaj nowe z modelu

            foreach (var ingredient in recipe.Ingredients)
            {
                var recipeIngredient = new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = ingredient.Name,
                    UnitId = ingredient.Unit,
                    Quantity = ingredient.Quantity
                };
                _ingredientService.AddCompleteIngredients(recipeIngredient);
            }           
		}
	}
}
