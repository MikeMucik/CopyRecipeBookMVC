﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.Test.Common;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Infrastructure.Repositories;

namespace CopyRecipeBookMVC.Application.Test.IntergrationTests.RecipeIntTests
{
	[Collection("QueryCollection")]
	public class GetRecipesByCategoryIntTests(QueryTestFixtures fixtures) : RecipeIntegrationView(fixtures)
	{
		[Fact]
        public void ReturnByCategory_GetRecipesByCategory_ReturnListRecipesByCategory()
        {
            //Arrange
            var category = 1;
            var pageNumber = 1;
            var pageSize = 2;
            //Act
            var result = _recipeService.GetRecipesByCategory(pageSize, pageNumber, category);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.IsType<ListRecipesByCategoryVm>(result);
        }
		[Fact]
		public void ReturnByNotExistingCategory_GetRecipesByCategory_ReturnZeroRecipes()
		{
			//Arrange
			var category = 0;
			var pageNumber = 1;
			var pageSize = 2;
			//Act
			var result = _recipeService.GetRecipesByCategory(pageSize, pageNumber, category);
			//Assert
			Assert.Equal([],result.RecipesByCategory);			
			Assert.IsType<ListRecipesByCategoryVm>(result);
		}

	}
}
