using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
	public class DifficultyRepository : IDifficultyRepository
	{
		private readonly Context _context;
        public DifficultyRepository(Context context)
        {
            _context = context;
        }
        public IEnumerable<Difficulty> GetAllDifficulties()
		{
			return _context.Difficulties.ToList();
		}
	}
}
