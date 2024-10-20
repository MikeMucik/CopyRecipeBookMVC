using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

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
		public int AddIngredient(IngredientForNewRecipeVm ingredient)
		{
			var ingredientNew = _mapper.Map<Ingredient>(ingredient);
			var id = _ingredientRepo.AddIngredient(ingredientNew);
			return id;
		}

		public List<SelectListItem> GetIngredientSelectList()
		{
			var ingredientListVm = GetListIngredientForList();
			return ingredientListVm.Ingredients.Select(ing => new SelectListItem
			{
				Value = ing.Id.ToString(),
				Text = ing.Name,
			}).ToList();				
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
			if (ingredient.Name > 0)
			{
				return ingredient.Name; 
			}			
			if (!string.IsNullOrEmpty(ingredient.NewIngredientName))
			{
				var existingIngredient = _ingredientRepo.ExistingIngredient(ingredient.NewIngredientName);
				if (existingIngredient != null)
				{
					return existingIngredient.Id;
				}
				return AddIngredient(new IngredientForNewRecipeVm { NewIngredientName = ingredient.NewIngredientName });
			}
			else
			{
				Console.WriteLine("Błąd");
				return -1;
			}
		}		
    }
}
