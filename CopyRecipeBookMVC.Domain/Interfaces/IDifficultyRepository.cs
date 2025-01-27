﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Domain.Interfaces
{
	public interface IDifficultyRepository
	{
		IEnumerable<Difficulty> GetAllDifficulties();
		int GetIdByName(string name);
	}
}
