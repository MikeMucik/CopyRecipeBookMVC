﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Domain.Interfaces
{
	public interface IIngredientRepository
	{
		int AddIngredient (Ingredient ingredient);		
		IQueryable<Ingredient> GetAllIngredients ();
		Ingredient GetIngredientById (int id);		     
		Ingredient ExistingIngredient(string name);		
    }
}
