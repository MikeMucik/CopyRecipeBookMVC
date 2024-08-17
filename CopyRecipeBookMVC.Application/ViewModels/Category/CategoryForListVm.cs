using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.ViewModels.Category
{
	public class CategoryForListVm : IMapFrom<Domain.Model.Category>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public void Mapping(Profile profile)
		{ 
			profile.CreateMap<Domain.Model.Category, CategoryForListVm >();
		}
	}
}
