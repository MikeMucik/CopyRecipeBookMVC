using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;


namespace CopyRecipeBookMVC.Application.Services
{
	public class RecipeIngredientService : IRecipeIngredientService
	{
		private readonly IRecipeIngredientRepository _recipeIngredientRepo;
		private readonly IMapper _mapper;
		public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepo, IMapper mapper)
		{
			_recipeIngredientRepo = recipeIngredientRepo;
			_mapper = mapper;
		}
		public void AddCompleteIngredients(RecipeIngredient recipeIngredient)
		{
			_recipeIngredientRepo.AddCompleteIngredients(recipeIngredient);
		}		
		public void DeleteCompleteIngredients(int recipeId)
		{
			var ingredientsToDelete = _recipeIngredientRepo.GetAllIngredientsById(recipeId);
			foreach (var item in ingredientsToDelete)
			{
				_recipeIngredientRepo.DeleteCompleteIngredient(item);
			}
		}
	}
}
