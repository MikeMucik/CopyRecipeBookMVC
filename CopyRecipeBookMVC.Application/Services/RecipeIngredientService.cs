using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;


namespace CopyRecipeBookMVC.Application.Services
{
	public class RecipeIngredientService : IRecipeIngredient
	{
		private readonly IIngredientRepository _ingredientRepo;
		private readonly IMapper _mapper;
		public RecipeIngredientService(IIngredientRepository ingredientRepo, IMapper mapper)
		{
			_ingredientRepo = ingredientRepo;
			_mapper = mapper;
		}
		public void AddCompleteIngredients(RecipeIngredient recipeIngredient)
		{
			_ingredientRepo.AddCompleteIngredients(recipeIngredient);
		}
		public int AddIngredient(IngredientForNewRecipeVm ingredient)
		{
			var ingredientNew = _mapper.Map<Ingredient>(ingredient);
			var id = _ingredientRepo.AddIngredient(ingredientNew);
			return id;
		}
		public void DeleteCompleteIngredients(int recipeId)
		{
			var ingredientsToDelete = _ingredientRepo.GetAllIngredientsById(recipeId);
			foreach (var item in ingredientsToDelete)
			{
				_ingredientRepo.DeleteCompleteIngredient(item);
			}
		}
	}
}
