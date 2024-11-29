using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using FluentValidation.TestHelper;

namespace CopyRecipeBookMVC.Application.Test.Validations
{
	public class NewRecipeValidatorTest
	{
		[Fact]
		public void Add_NewRecipe_ProperDate_ShouldNotReturnValidationError()
		{
			var validator = new NewRecipeValidation();
			var command = new NewRecipeVm
			{
				Id = 1,
				Name = "Test",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Description = "Test",
				NumberOfIngredients = 1	
			};
			validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
		}
		[Fact]
		public void Add_NewRecipe_InvalidId_ShouldReturnValidationErrorForId()
		{
			var validator = new NewRecipeValidation();
			var command = new NewRecipeVm
			{
				Id = 0,				
			};
			validator.TestValidate(command).ShouldHaveValidationErrorFor(nameof(NewRecipeVm.Id));
		}
		//Stworzyć tyle testów co jest RuleFor i analogicznie dla IngredientForNewRecipeVm
	}
}
