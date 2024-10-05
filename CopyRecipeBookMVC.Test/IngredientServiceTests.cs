using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Test
{
	public class IngredientServiceTests
	{
		[Fact]
		public void Add_AddIngredient_ShouldAddIngredientNameToCollection()
		{
			//Arrange
			var newIngredient = new Ingredient
			{
				Id = 1,
				Name = "Test",
			};
			var newIngredientVm = new IngredientForNewRecipeVm
			{
				NewIngredientName = "Test",
				NewIngredientUnit = "TestUnit",
			};
			var mockRepo = new Mock<IIngredientRepository>();
			mockRepo
				.Setup(repo => repo.AddIngredient(It.IsAny<Ingredient>()))
				.Returns(1);
			var mockMapper = new Mock<IMapper>();
			mockMapper
				.Setup(mapper => mapper.Map<Ingredient>(It.IsAny<IngredientForNewRecipeVm>()))
				.Returns(newIngredient);
			var mockService = new IngredientService(mockRepo.Object, mockMapper.Object);
			//Act
			var result = mockService.AddIngredient(newIngredientVm);
			//Assert
			mockRepo.Verify(repo => repo.AddIngredient(It.Is<Ingredient>(i => i.Name == "Test")), Times.Once);
			Assert.Equal(1, result);
		}
		[Fact]
		public void Remove_DeleteCompleteIngredient_RemoveFromListCompleteIngredients()
		{
			//Arrange
			var recipeId = 1;
			var completeIngredient1 = new RecipeIngredient
			{
				RecipeId = recipeId,
				IngredientId = 1,
				UnitId = 1,
				Quantity = 1,
			};
			var completeIngredient2 = new RecipeIngredient
			{
				RecipeId = recipeId,
				IngredientId = 2,
				UnitId = 2,
				Quantity = 2,
			};
			var ingredientsToDelete = new List<RecipeIngredient>
			{
				completeIngredient1, completeIngredient2
			};

			var mockRepo = new Mock<IIngredientRepository>();
			mockRepo
				.Setup(repo => repo.GetAllIngredientsById(recipeId))
				.Returns(ingredientsToDelete);
			var mockMapper = new Mock<IMapper>();
							
			var mockService = new IngredientService(mockRepo.Object, mockMapper.Object);
			//Act
			mockService.DeleteCompleteIngredients(recipeId);
			//Assert
			mockRepo.Verify(repo => repo.DeleteCompleteIngredient(It.Is<RecipeIngredient>(r => r.IngredientId == completeIngredient1.IngredientId)), Times.Once);
			mockRepo.Verify(repo => repo.DeleteCompleteIngredient(It.Is<RecipeIngredient>(r => r.IngredientId == completeIngredient2.IngredientId)), Times.Once);
		}
	}

}
