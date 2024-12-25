using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Difficulty;
using CopyRecipeBookMVC.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Services
{
	public class DifficultyService : IDifficultyService
	{
		private readonly IDifficultyRepository _difficultyRepo;
		private readonly IMapper _mapper;
        public DifficultyService(IDifficultyRepository difficultyRepo, IMapper mapper)
        {
			_difficultyRepo = difficultyRepo;
			_mapper = mapper;
        }

		

		public ListDifficultyForListVm GetListDifficultyForList()
		{
			var difficulties = _difficultyRepo.GetAllDifficulties();
			var difficultyVms = _mapper.Map<List<DifficultyForListVm>>(difficulties);
			var difficultyList = new ListDifficultyForListVm()
			{
				Difficulties = difficultyVms,
			};
			return difficultyList;
		}
		public List<SelectListItem> GetDifficultySelectList()
		{
			var difficultyListVm = GetListDifficultyForList();
			return difficultyListVm.Difficulties.Select(difficultyListVm => new SelectListItem
			{
				Value = difficultyListVm.Id.ToString(),
				Text = difficultyListVm.Name,	
			}).ToList();
		}
		public int GetDifficultyIdByName(string name)
		{
			int id = _difficultyRepo.GetIdByName(name);
			return id;
		}
	}
}
