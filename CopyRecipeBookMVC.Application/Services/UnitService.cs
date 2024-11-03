using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Application.ViewModels.Unit;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CopyRecipeBookMVC.Application.Services
{
	public class UnitService : IUnitService
	{
		private readonly IUnitRepository _unitRepo;
		private readonly IMapper _mapper;
		public UnitService(IUnitRepository unitRepo, IMapper mapper)
		{
			_unitRepo = unitRepo;
			_mapper = mapper;
		}
		public int AddUnit(IngredientForNewRecipeVm unit)
		{
			var unitNew = _mapper.Map<Unit>(unit);
			var id = _unitRepo.AddUnit(unitNew);
			return id;
		}
		public List<SelectListItem> GetUnitsForSelectList()
		{
			var unit = _unitRepo.GetAllUnits();
			var unitVms = _mapper.Map<List<UnitForListVm>>(unit);
			var unitList = new ListUnitForListVm()
			{
				Units = unitVms
			};
			return unitList.Units.Select(uni => new SelectListItem
			{
				Value = uni.Id.ToString(),
				Text = uni.Name	
			}).ToList();
		}
		public int GetOrAddUnit(IngredientForNewRecipeVm ingredient)
		{
			if (ingredient.IngredientUnit > 0)
			{
				return ingredient.IngredientUnit;
			}
			if (!string.IsNullOrEmpty(ingredient.NewIngredientUnit))
			{
				var existingUnit = _unitRepo.ExistingUnit(ingredient.NewIngredientUnit);
				if (existingUnit != null)
				{
					return existingUnit.Id;
				}
				else
				{
					return AddUnit(new IngredientForNewRecipeVm { NewIngredientUnit = ingredient.NewIngredientUnit });
				}
			}
			else
			{
				return -1;
			}
		}
	}
}
