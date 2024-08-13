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
		private readonly IMapper _mapper;
        public RecipeService(IRecipeRepository recipeRepo, IMapper mapper)
        {
            _recipeRepo = recipeRepo;
			_mapper = mapper;
        }
        public int AddRecipe(NewRecipeVm recipe)
		{
			throw new NotImplementedException();
		}

		public void DeleteRecipe(int id)
		{
			throw new NotImplementedException();
		}

		public ListRecipeForListVm GetAllRecipesForList()
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider).ToList();

			var recipeList = new ListRecipeForListVm()
			{
				Recipes = recipes,
				Count = recipes.Count()
			};

			return recipeList;
		}

		public RecipeDetailsVm GetRecipe(int id)
		{
			var recipe = _recipeRepo.GetRecipeById(id);
			var recipeVm = _mapper.Map<RecipeDetailsVm>(recipe);
			return recipeVm;
		}

		public ListRecipeForListVm GetRecipesByCategory(int categoryId)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r =>  categoryId == r.CategoryId)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider).ToList();

			var recipeList = new ListRecipeForListVm()
			{
				Recipes = recipes,
				Count = recipes.Count()
			};

			return recipeList;
		}

		public ListRecipeForListVm GetRecipesByDifficulty(int difficultyId)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r => difficultyId == r.DifficultyId)
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
