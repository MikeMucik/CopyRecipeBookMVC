using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsService.RecipeIngredientSeviceTests
{
    public class RecipeIngredientTests
    {
        [Fact]
        public void Add_AddCompleteIngredients()
        {

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

            var mockRecipeRepo = new Mock<IRecipeIngredientRepository>();
            mockRecipeRepo
                .Setup(repo => repo.GetAllIngredientsById(recipeId))
                .Returns(ingredientsToDelete);
            var mockMapper = new Mock<IMapper>();

            var mockService = new RecipeIngredientService(mockRecipeRepo.Object, mockMapper.Object);
            //Act
            mockService.DeleteCompleteIngredients(recipeId);
            //Assert
            mockRecipeRepo.Verify(repo => repo.DeleteCompleteIngredient(It.Is<RecipeIngredient>(r => r.IngredientId == completeIngredient1.IngredientId)), Times.Once);
            mockRecipeRepo.Verify(repo => repo.DeleteCompleteIngredient(It.Is<RecipeIngredient>(r => r.IngredientId == completeIngredient2.IngredientId)), Times.Once);
        }
    }
}
