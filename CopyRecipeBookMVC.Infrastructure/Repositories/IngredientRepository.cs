using System;
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
        public int AddIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
            return ingredient.Id;
        }       
        public IQueryable<Ingredient> GetAllIngredients()
        {
            return _context.Ingredients;
        }
        public Ingredient GetIngredientById(int id)
        {
            var ingredient = _context.Ingredients.FirstOrDefault(x => x.Id == id);
            return ingredient;        }
        
        public Ingredient ExistingIngredient(string name)
        {
            return _context.Ingredients.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());
        }        
    }
}
