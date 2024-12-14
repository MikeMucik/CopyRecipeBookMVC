using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
	public class TimeRepository : ITimeRepositoy
	{
		private readonly Context _context;
		public TimeRepository(Context context)
		{
			_context = context;
		}		
		public int AddTime(Time time)
		{
			_context.Times.Add(time);
			_context.SaveChanges();
			return time.Id;
		}
		//public Time GetTimeById(int id)
		//{
		//	return _context.Times.Find(id);
		//}
		public IQueryable<Time> GetAllTimes()
		{
			return _context.Times;
		}
		public Time ExistingTime(decimal? timeAmount, string TimeUnit)
		{
			return _context.Times.FirstOrDefault(t => t.Amount == timeAmount && t.Unit == TimeUnit);
		}
	}
}
