using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests
{
    [Collection("QueryCollection")]
    public class RecipeIntegrationView : CommandTestBase//żeby sprawdzić głębiej ;)
    {
        public readonly RecipeService _recipeService;
        public readonly RecipeRepository _recipeRepo;
        public readonly IMapper _mapper;

        //private readonly Context _context;
        public readonly IRecipeIngredientService _recipeIngredientService;
        public readonly IIngredientService _ingredientService;
        public readonly IUnitService _unitService;

        public RecipeIntegrationView(QueryTestFixtures fixtures)
        {
            var _context = fixtures.Context;
            var MapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = MapperConfig.CreateMapper();

            _recipeRepo = new RecipeRepository(_context);
            var _ingredientRepo = new IngredientRepository(_context);
            var _unitRepo = new UnitRepository(_context);
            var _recipeIngredientRepo = new RecipeIngredientRepository(_context);

            _ingredientService = new IngredientService(_ingredientRepo, _mapper);
            _unitService = new UnitService(_unitRepo, _mapper);
            _recipeIngredientService = new RecipeIngredientService(_recipeIngredientRepo, _mapper);
            _recipeService = new RecipeService(_recipeRepo, _mapper, _recipeIngredientService);
        }
    }
}
