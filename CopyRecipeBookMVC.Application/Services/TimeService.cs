﻿using System;
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
			var listOfTimes = GetListTimeForList();
			foreach (var item in listOfTimes.Times)
			{
				if ((time.TimeAmount == item.Amount) && (time.TimeUnit == item.Unit))
				{
					return item.Id;
				}
			}
			var timeNew = _mapper.Map<Time>(time);
			var id = _timeRepo.AddTime(timeNew);

			return id;
		}

		public Time GetTime(int id)
		{
			throw new NotImplementedException();
		}

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
	}
}
