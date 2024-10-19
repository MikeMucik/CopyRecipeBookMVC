using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Unit;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Test.UnitTests
{
    public class UnitServiceTests
    {

        [Fact]
        public void Add_AddUnit_ShouldAddIngredientUnitToCollection()
        {
            //Arrange
            var newUnit = new Unit
            {
                Id = 1,
                Name = "TestUnit",
            };
            var newUnitVm = new IngredientForNewRecipeVm
            {
                //NewIngredientName = "Test",
                NewIngredientUnit = "TestUnit"
            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(repo => repo.AddUnit(It.IsAny<Unit>()))
                .Returns(1);
            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(mapper => mapper.Map<Unit>(It.IsAny<IngredientForNewRecipeVm>()))
                .Returns(newUnit);
            var mockService = new UnitService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.AddUnit(newUnitVm);
            //Assert
            mockRepo.Verify(repo => repo.AddUnit(It.Is<Unit>(i => i.Name == "TestUnit")), Times.Once);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Get_GetAllUnitsForList_ShouldGetAllList()
        {
            //Arrange
            var units = new List<Unit>
            {
                new Unit { Id = 1, Name="TestU1" },
                new Unit { Id = 2,  Name="TestU2"}
            };
            var unitVms = new List<UnitForListVm>
            {
                new UnitForListVm { Id = 1, Name="TestU1" },
                new UnitForListVm { Id = 2, Name="TestU2" }

            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(repo => repo.GetAllUnits())
                .Returns(units.AsQueryable);
            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(map => map.Map<List<UnitForListVm>>(units))
                .Returns(unitVms);
            var mockService = new UnitService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetAllUnitsForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListUnitForListVm>(result);
            Assert.Equal(2, result.Units.Count);
            Assert.Contains(result.Units, c => c.Name == "TestU1");
            Assert.Contains(result.Units, c => c.Name == "TestU2");
        }
        [Fact]
        public void GetOrAdd_GetOrAddUnit_ShouldAddNewUnit()
        {
            //Assert
            var newUnitVm = new IngredientForNewRecipeVm
            {
                Unit = 0,
                NewIngredientUnit = "kg"
            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(i => i.ExistingUnit(newUnitVm.NewIngredientUnit))
                .Returns((Unit)null);
            mockRepo
                .Setup(u => u.AddUnit(It.IsAny<Unit>()))
                .Returns(1);
            var mockMapper = new Mock<IMapper>();
            var mockService = new UnitService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetOrAddUnit(newUnitVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            mockRepo.Verify(i => i.AddUnit(It.IsAny<Unit>()), Times.Once);
        }
        [Fact]
        public void GetOrAdd_GetOrAddUnit_ShouldGetBackIdUnitByName()
        {
            //Assert
            var newUnitVm = new IngredientForNewRecipeVm
            {
                NewIngredientUnit = "kg"
            };
            var newUnit = new Unit
            {
                Id = 2,
                Name = "kg"
            };
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo
                .Setup(u => u.ExistingUnit(newUnitVm.NewIngredientUnit))
                .Returns(newUnit);
            var mockMapper = new Mock<IMapper>();
            var mockService = new UnitService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetOrAddUnit(newUnitVm);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result);
        }

    }
}

