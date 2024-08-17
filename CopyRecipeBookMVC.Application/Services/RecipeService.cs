using System;
using System.Collections.Generic;
using System.Linq;
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
		public RecipeService(IRecipeRepository recipeRepo, 
			IIngredientRepository ingredientRepo,
			ICategoryRepository categoryRepo,
			IMapper mapper)
		{
			_recipeRepo = recipeRepo;
			_ingredientRepo = ingredientRepo;
			_mapper = mapper;
			_categoryRepo = categoryRepo;
		}
		public int AddRecipe(NewRecipeVm recipe)
		{
			throw new NotImplementedException();
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
				PageSize=pageSize,
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
