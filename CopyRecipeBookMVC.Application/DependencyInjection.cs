﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CopyRecipeBookMVC.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAplication(this IServiceCollection services)
		{
			services.AddTransient<IRecipeService, RecipeService>();
			services.AddTransient<IIngredientService, IngredientService>();
			services.AddTransient<ITimeService, TimeService>();
			services.AddTransient<ICategoryService, CategoryService>();
			services.AddTransient<IDifficultyService, DifficultyService>();
			services.AddTransient<IUnitService, UnitService>();
			services.AddTransient<IRecipeIngredientService,  RecipeIngredientService>();
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			return services;
		}
	}
}
