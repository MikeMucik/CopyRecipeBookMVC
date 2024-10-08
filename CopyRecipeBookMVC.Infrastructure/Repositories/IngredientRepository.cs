﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
	public class IngredientRepository : IIngredientRepository
	{
		private readonly Context _context;
        public IngredientRepository(Context context)
        {
            _context = context;
        }
        public int AddIngredient (Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
            return ingredient.Id;
        }
        public void AddCompleteIngredients(RecipeIngredient recipeIngredient)
        {
            _context.RecipeIngredient.Add(recipeIngredient);
            _context.SaveChanges();            
        }
        public IQueryable<Ingredient> GetAllIngredients ()
        {
            return _context.Ingredients;
        }
        public Ingredient GetIngredientById (int id)
        {
            var ingredient = _context.Ingredients.FirstOrDefault(x => x.Id == id);
            return ingredient;
        }
        public int AddUnit (Unit unit)
        {
            _context.Units.Add(unit);
            _context.SaveChanges ();
            return unit.Id;
        }

        public IQueryable<Unit> GetAllUnits ()
        {
            return _context.Units;
        }
        public Unit GetUnitById (int id)
        {
            var unit = _context.Units.FirstOrDefault(x => x.Id == id);
            return unit;
        }
        public IEnumerable<RecipeIngredient> GetAllIngredientsById(int recipeId)
        {
            return _context.RecipeIngredient.Where(ri => ri.RecipeId == recipeId)
                                            .ToList();
        }
        public void DeleteCompleteIngredient(RecipeIngredient item)
        {
            _context.RecipeIngredient.Remove(item);
            _context.SaveChanges ();
        }
    }
}
