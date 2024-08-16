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
		private readonly IICategoryRepository _categoryRepo;
		public RecipeService(IRecipeRepository recipeRepo, 
			IIngredientRepository ingredientRepo,
			IICategoryRepository categoryRepo,
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

		public ListRecipesByCategoryVm GetRecipesByCategory(int pageSize, int pageNumber, string categoryName)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r => categoryName == r.Category.Name)
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
				CategoryId = categoryName,
				Count = recipes.Count,
				Categories = _categoryRepo.GetAllCategories()	
					.Select(c => new SelectListCategories
						{
							Value = c.Name,  // Zmieniono na nazwę
							Text = c.Name
						})
					.ToList()
			};
			//if (recipes.Count == 0)
			//{
			//	recipeList = String.Empty;
			//}
			return recipeList;
		}

		public ListRecipeForListVm GetRecipesByDifficulty(int difficultyId)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r => difficultyId == r.Difficulty.Id)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider).ToList();

			var recipeList = new ListRecipeForListVm()
			{
				Recipes = recipes,
				Count = recipes.Count()
			};

			return recipeList;
		}

		public int UpdaterRecipe(Recipe recipe)
		{
			throw new NotImplementedException();
		}
	}
}
