using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Category;
using CopyRecipeBookMVC.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepo;
		private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
			_mapper = mapper;
        }
        public ListCategoryForListVm GetListCategoryForList()
		{
			var categories = _categoryRepo.GetAllCategories();
			var categoryVms = _mapper.Map<List<CategoryForListVm>>(categories);
			var categoryList = new ListCategoryForListVm
			{
				Categories = categoryVms,
			};
			return categoryList;
		}
		public List<SelectListItem> GetCategoryForSelectList()
		{
			var categories = _categoryRepo.GetAllCategories();
			var categoriesVms = _mapper.Map<List<CategoryForListVm>>(categories);
			return categoriesVms.Select(uniVm => new SelectListItem
				{
					Value = uniVm.Id.ToString(),
					Text = uniVm.Name
				}).ToList();

			//var categories = _categoryRepo.GetAllCategories();
			//var categoryVms = _mapper.Map<List<CategoryForListVm>>(categories);
			////var categoryListVm= GetListCategoryForList();
			//var categoryListVm = new ListCategoryForListVm
			//{
			//	Categories = categoryVms
			//};
			//return categoryListVm.Categories.Select(cat => new SelectListItem
			//{
			//	Value = cat.Id.ToString(),
			//	Text = cat.Name,
			//}).ToList();
		}
	}
}
