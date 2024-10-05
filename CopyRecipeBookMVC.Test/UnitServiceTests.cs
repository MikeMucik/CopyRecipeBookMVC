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
					NewIngredientName = "Test",
					NewIngredientUnit = "TestUnit",
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

		}
	}

