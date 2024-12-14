using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using CopyRecipeBookMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Application.Mapping;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests
{
	public class RecipeIntegrationCommand : CommandTestBase
	{
		public readonly DbContextOptions<Context> _contextOptions;
		public readonly RecipeService _recipeService;
		public readonly IIngredientService _ingredientService;
		public readonly IUnitService _unitService;
		public readonly IMapper _mapper;
		public RecipeIntegrationCommand() : base()
		{
			_contextOptions = new DbContextOptionsBuilder<Context>()
			.UseInMemoryDatabase("SharedTestDatabase")
			.Options;

			var MapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			_mapper = MapperConfig.CreateMapper();
			var _recipeRepo = new RecipeRepository(_context);
			var _ingredientRepo = new IngredientRepository(_context);
			var _unitRepo = new UnitRepository(_context);
			var _recipeIngredientRepo = new RecipeIngredientRepository(_context);

			_ingredientService = new IngredientService(_ingredientRepo, _mapper);
			_unitService = new UnitService(_unitRepo, _mapper);
			var _recipeIngredientService = new RecipeIngredientService(_recipeIngredientRepo, _mapper);
			_recipeService = new RecipeService(_recipeRepo, _mapper, _recipeIngredientService);
		}
	}
}
