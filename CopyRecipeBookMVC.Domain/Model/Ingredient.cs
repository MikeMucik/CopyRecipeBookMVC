﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Domain.Model
{
	public class Ingredient
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<RecipeIngredient> RecipeIngredient { get;}
	}
}
