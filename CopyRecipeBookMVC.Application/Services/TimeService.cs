using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.Time;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Services
{
	public class TimeService : ITimeService
	{
		private readonly ITimeRepositoy _timeRepo;
		private readonly IMapper _mapper;
		public TimeService(ITimeRepositoy timeRepo, IMapper mapper)
		{
			_timeRepo = timeRepo;
			_mapper = mapper;
		}
		public int AddTime(NewRecipeVm time)
		{
			//sprawdzenie czy taki czas już jest
			var existingTime = _timeRepo.ExistingTime(time.TimeAmount, time.TimeUnit);
			if (existingTime != null)
			{
				return existingTime.Id;
			}
			var timeNew = _mapper.Map<Time>(time);
			var id = _timeRepo.AddTime(timeNew);

			return id;
		}

		//public Time GetTime(int id)
		//{
		//	throw new NotImplementedException();
		//}

		public ListTimeForListVm GetListTimeForList()
		{
			var times = _timeRepo.GetAllTimes();
			var timeVms = _mapper.Map<List<TimeForListVm>>(times);
			var timeList = new ListTimeForListVm()
			{
				Times = timeVms
			};
			return timeList;
		}

		public List<SelectListItem> GetTimeSelectItem()
		{
			var timeListVm = GetListTimeForList();
			return timeListVm.Times.Select(tim => new SelectListItem
			{
				Value = tim.Id.ToString(),
				Text = tim.Amount.ToString() + " " + tim.Unit ,
			}).ToList();
		}
	}
}
