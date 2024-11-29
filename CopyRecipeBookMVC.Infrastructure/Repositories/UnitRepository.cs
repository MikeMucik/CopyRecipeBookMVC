﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Infrastructure.Repositories
{
    public class UnitRepository :IUnitRepository
    {
        private readonly Context _context;
        public UnitRepository(Context context) 
        { 
            _context = context;
        }
		public int AddUnit(Unit unit)
		{
			_context.Units.Add(unit);
			_context.SaveChanges();
			return unit.Id;
		}
		public IQueryable<Unit> GetAllUnits()
		{
			return _context.Units;
		}
		public Unit GetUnitById(int id)
		{
			var unit = _context.Units.FirstOrDefault(x => x.Id == id);
			return unit;
		}
		public Unit ExistingUnit(string name)
		{
			return _context.Units.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());
		}
	}
}