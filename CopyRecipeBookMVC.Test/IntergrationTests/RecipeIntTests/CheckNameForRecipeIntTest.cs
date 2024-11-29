using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
    [Collection("QueryCollection")]
    public class CheckNameForRecipeIntTest 
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeRepository _recipeRepo;

        public CheckNameForRecipeIntTest(QueryTestFixtures fixtures) 
        {
            var _context = fixtures.Context;
            _recipeRepo = new RecipeRepository(_context);
            _recipeService = new RecipeService(_recipeRepo, null, null);
        }

        [Fact]
        public void CheckExistingRecipe_CheckNameForRecipe_ReturnTrue()
        {
            //Arrange
            var name = "Test";
            //Act
            var result = _recipeService.CheckNameForRecipe(name);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void CheckNotExistingRecipe_CheckNameForRecipe_ReturnFalse()
        {
            //Arrange
            var name = "NotTest";
            //Act
            var result = _recipeService.CheckNameForRecipe(name);
            //Assert
            Assert.False(result);
        }

    }
}
