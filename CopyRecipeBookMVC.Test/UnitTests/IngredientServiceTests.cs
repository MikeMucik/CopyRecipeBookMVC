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

namespace CopyRecipeBookMVC.Test.UnitTests
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
        public void Get_GetListIngredientForList()
        {
            //Arrange
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name="TestI1" },
                new Ingredient { Id = 2,  Name="TestI2"}
            };
            var ingredientsVms = new List<IngredientForListVm>
            {
                new IngredientForListVm { Id = 1, Name="TestI1" },
                new IngredientForListVm { Id = 2, Name="TestI2" }
            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(repo => repo.GetAllIngredients())
                .Returns(ingredients.AsQueryable);
            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(map => map.Map<List<IngredientForListVm>>(ingredients))
                .Returns(ingredientsVms);
            var mockService = new IngredientService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetListIngredientForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListIngredientsForRecipeVm>(result);
            Assert.Equal(2, result.Ingredients.Count);
            Assert.Contains(result.Ingredients, c => c.Name == "TestI1");
            Assert.Contains(result.Ingredients, c => c.Name == "TestI2");
        }

        [Fact]
        public void GetOrAdd_GetOrAddIngredient_ShouldAddNewIngredientNameWhenIngredientNotSelected()
        {
            //Assert
            var newIngredientNameVm = new IngredientForNewRecipeVm
            {
                Name = 0,
                NewIngredientName = "potato"
            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(i => i.ExistingIngredient(newIngredientNameVm.NewIngredientName))
                .Returns((Ingredient)null);
            mockRepo
                .Setup(i => i.AddIngredient(It.IsAny<Ingredient>()))
                .Returns(1);
            var mockMapper = new Mock<IMapper>();
            var mockService = new IngredientService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetOrAddIngredient(newIngredientNameVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            mockRepo.Verify(i => i.AddIngredient(It.IsAny<Ingredient>()), Times.Once);
        }
        [Fact]
        public void GetOrAdd_GetOrAddIngredient_ShouldGetBackIdIngredientByName()
        {
            //Assert
            var newIngredientNameVm = new IngredientForNewRecipeVm
            {
                NewIngredientName = "potato"
            };
            var newIngredientName = new Ingredient
            {
                Id = 2,
                Name = "potato"
            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(i => i.ExistingIngredient(newIngredientNameVm.NewIngredientName))
                .Returns(newIngredientName);
            var mockMapper = new Mock<IMapper>();
            var mockService = new IngredientService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetOrAddIngredient(newIngredientNameVm);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result);
        }
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
