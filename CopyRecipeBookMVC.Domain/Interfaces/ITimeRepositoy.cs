using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Domain.Interfaces
{
	public interface ITimeRepositoy
	{
		int AddTime(Time time);
		//Time GetTimeById(int id);
		IQueryable<Time> GetAllTimes();
		Time ExistingTime(decimal? timeAmount, string TimeUnit);
	}
}
