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
	public class TryAddRecipeIntTests(QueryTestFixtures fixtures) : RecipeIntegrationView(fixtures)
	{
		[Fact]
		public void ExistingRecipe_TryAddRecipe_ReturnIdExisting()
		{
			//Arrange
			var tryRecipe = new NewRecipeVm
			{
				Name = "Test",
			};
			//Act
			var result = _recipeService.TryAddRecipe(tryRecipe);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(1, result);
		}
		[Fact]
		public void NotExistingRecipe_TryAddRecipe_ReturnNull()
		{
			//Arrange
			var tryRecipe = new NewRecipeVm
			{
				Name = "NotTest",
			};
			//Act
			var result = _recipeService.TryAddRecipe(tryRecipe);
			//Assert
			Assert.Null(result);
		}
	}
}
