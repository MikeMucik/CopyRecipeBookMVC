using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Category;
using CopyRecipeBookMVC.Domain.Interfaces;

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
			var categogoryList = new ListCategoryForListVm
			{
				Categories = categoryVms,
			};
			return categogoryList;
		}
	}
}
