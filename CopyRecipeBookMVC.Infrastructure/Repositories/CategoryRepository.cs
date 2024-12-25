using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly Context _context;
        public CategoryRepository(Context context)
        {
            _context = context;
        }
        public IEnumerable<Category> GetAllCategories()
		{
			return _context.Categories.ToList();
		}
		public int GetIdByName(string name) 
		{ 
			return _context.Categories.FirstOrDefault(c => c.Name == name).Id;
		}
	}
}
