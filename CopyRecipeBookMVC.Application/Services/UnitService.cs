using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.Unit;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.Services
{
    public class UnitService :IUnitService
    {
        private readonly IIngredientRepository _ingredientRepo;
        private readonly IMapper _mapper;
        public UnitService(IIngredientRepository ingredientRepo, IMapper mapper)
        {
            _ingredientRepo= ingredientRepo;
            _mapper = mapper;
        }
        public int AddUnit(IngredientForNewRecipeVm unit)
        {
            var unitNew = _mapper.Map<Unit>(unit);
            var id  =_ingredientRepo.AddUnit(unitNew);
            return id;
        }

        public ListUnitForListVm  GetAllUnitsForList()
        {
            var unit = _ingredientRepo.GetAllUnits();
            var unitVms = _mapper.Map<List<UnitForListVm>>(unit);
            var unitList = new ListUnitForListVm()
            {
                Units = unitVms
            };
            return unitList;
        }

        public Unit GetUnit(int id)
        {
            throw new NotImplementedException();
        }
    }
}
