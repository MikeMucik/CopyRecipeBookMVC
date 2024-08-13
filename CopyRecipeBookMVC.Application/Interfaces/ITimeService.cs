using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Interfaces
{
	public interface ITimeService
	{
		int AddTime (Time time);
		Time GetTime (int id);
		List<Time> GetTimeList ();
	}
}
