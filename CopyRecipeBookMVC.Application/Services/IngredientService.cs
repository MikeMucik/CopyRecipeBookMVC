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
	public class IngredientService : IIngredientService
	{
		private readonly IIngredientRepository _ingredientRepo;
		private readonly IMapper _mapper;
        public IngredientService(IIngredientRepository ingredientRepo, IMapper mapper)
		{ 
			_ingredientRepo = ingredientRepo;
			_mapper = mapper;
		}

		public void AddCompleteIngredients (RecipeIngredient recipeIngredient)
		{
			var completeIngredient = _mapper.Map<RecipeIngredient>(recipeIngredient);
			_ingredientRepo.AddCompleteIngredients(completeIngredient);

		}
        
        public int AddIngredient(IngredientForNewRecipeVm ingredient)
		{
			var ingredientNew = _mapper.Map<Ingredient>(ingredient);
			var id = _ingredientRepo.AddIngredient(ingredientNew);
			return id;
		}		

		public ListIngredientsForRecipeVm GetListIngredientForList()
		{
			var ingredient = _ingredientRepo.GetAllIngredients();
			var ingredientVms = _mapper.Map<List<IngredientForListVm>>(ingredient);
			var ingredientList = new ListIngredientsForRecipeVm()
			{
				Ingredients = ingredientVms,
			};
			return ingredientList;
		}

		public int GetOrAddIngredient(IngredientForNewRecipeVm ingredient)
		{
			//sprawdzenie czy nazwa składnika istnieje w bazie
			var listOfIngredient = GetListIngredientForList();
			foreach (var ing in listOfIngredient.Ingredients)

			{
				if (ing.Name == ingredient.NewIngredientName)
				{
					return ing.Id;
				}

			}
			if (string.IsNullOrEmpty(ingredient.NewIngredientName))
			{
				return ingredient.Name; // Zakładając, że ingredient.Name to ID istniejącego składnika
			}
			else
			{
				return AddIngredient(new IngredientForNewRecipeVm { NewIngredientName = ingredient.NewIngredientName });
			}
		}

		public Ingredient GetIngredient(int id)
		{
			throw new NotImplementedException();
		}		
	}
}
