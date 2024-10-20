using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Test.UnitTests
{
    public class IngredientServiceTests
    {
        private readonly Mock<IIngredientRepository> _ingredientRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IngredientService _ingredientService;
        public IngredientServiceTests()
        {
            _ingredientRepoMock = new Mock<IIngredientRepository>();
            _mapperMock = new Mock<IMapper>();
            _ingredientService = new IngredientService(_ingredientRepoMock.Object, _mapperMock.Object);
        }
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
            _ingredientRepoMock.Setup(repo => repo.AddIngredient(It.IsAny<Ingredient>())).Returns(1);
            _mapperMock.Setup(mapper => mapper.Map<Ingredient>(It.IsAny<IngredientForNewRecipeVm>()))
                .Returns(newIngredient);            
            //Act
            var result = _ingredientService.AddIngredient(newIngredientVm);
            //Assert
            _ingredientRepoMock.Verify(repo => repo.AddIngredient(It.Is<Ingredient>(i => i.Name == "Test")), Times.Once);
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
            _ingredientRepoMock.Setup(repo => repo.GetAllIngredients()).Returns(ingredients.AsQueryable);
           _mapperMock.Setup(map => map.Map<List<IngredientForListVm>>(ingredients))
                .Returns(ingredientsVms);            
            //Act
            var result = _ingredientService.GetListIngredientForList();
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
            _ingredientRepoMock.Setup(i => i.ExistingIngredient(newIngredientNameVm.NewIngredientName))
                .Returns((Ingredient)null);
            _ingredientRepoMock.Setup(i => i.AddIngredient(It.IsAny<Ingredient>())).Returns(1);            
            //Act
            var result =_ingredientService.GetOrAddIngredient(newIngredientNameVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            _ingredientRepoMock.Verify(i => i.AddIngredient(It.IsAny<Ingredient>()), Times.Once);
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
            _ingredientRepoMock
                .Setup(i => i.ExistingIngredient(newIngredientNameVm.NewIngredientName))
                .Returns(newIngredientName);
            
            //Act
            var result = _ingredientService.GetOrAddIngredient(newIngredientNameVm);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result);
        }
              
    }

}
