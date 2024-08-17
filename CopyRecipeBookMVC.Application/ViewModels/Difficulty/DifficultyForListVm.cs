using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Difficulty
{
	public class DifficultyForListVm :IMapFrom<Domain.Model.Difficulty>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<Domain.Model.Difficulty, DifficultyForListVm>();
		}
	}
}
