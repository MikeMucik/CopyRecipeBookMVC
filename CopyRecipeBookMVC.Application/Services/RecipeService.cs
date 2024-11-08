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
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CopyRecipeBookMVC.Application.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly IRecipeRepository _recipeRepo;		
		private readonly IMapper _mapper;				
		private readonly IRecipeIngredientService _recipeIngredientService;
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
		public bool CheckNameForRecipe(string name)
		{
			var exist = _recipeRepo.FindByName(name);
			if(exist == null)
			{
				return false;
			} else { return true; }
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
			var recipes = _recipeRepo.GetAllRecipes()
				.OrderBy(x=>x.Name)
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
				.OrderBy(x => x.Name)
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

		public ListRecipesByIngredientsVm GetRecipesByIngredients(int pageSize, int pageNumber, 
			List<int> ingredientIds)
		{
			var recipes = _recipeRepo.GetAllRecipes()
				.Where(r => ingredientIds.All(id => r.RecipeIngredient.Any(ri => ri.IngredientId == id)))
				.OrderBy(x => x.Name)
				//.Skip(pageSize * (pageNumber - 1))
				//.Take(pageSize)
				.ProjectTo<RecipeListForVm>(_mapper.ConfigurationProvider)
				.ToList();
			//var totalRecipesCount = _recipeRepo.GetAllRecipes()
			//	.Where(r => r.RecipeIngredient.Any(i => i.IngredientId == ingredientId))
			//	.Count();
			var recipesToShow = recipes
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToList();
			var recipeList = new ListRecipesByIngredientsVm()
			{
				RecipesByIngredients = recipes,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				IngredientIds = ingredientIds,
				Count = recipes.Count
			};
			return recipeList;
		}
	}
}
