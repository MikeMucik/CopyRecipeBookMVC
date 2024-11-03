using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Unit;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;
using Microsoft.AspNetCore.Mvc.Rendering;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;

namespace CopyRecipeBookMVC.Test.UnitTests
{
	public class UnitServiceTests
	{
		private readonly Mock<IUnitRepository> _unitRepoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly UnitService _unitService;
		public UnitServiceTests()
		{
			_unitRepoMock = new Mock<IUnitRepository>();
			_mapperMock = new Mock<IMapper>();
			_unitService = new UnitService(_unitRepoMock.Object, _mapperMock.Object);
		}
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
				NewIngredientUnit = "TestUnit"
			};			
			_unitRepoMock.Setup(repo => repo.AddUnit(It.IsAny<Unit>())).Returns(1);
			_mapperMock.Setup(mapper => mapper.Map<Unit>(It.IsAny<IngredientForNewRecipeVm>()))
				.Returns(newUnit);
			//Act
			var result = _unitService.AddUnit(newUnitVm);
			//Assert
			_unitRepoMock.Verify(repo => repo.AddUnit(It.Is<Unit>(i => i.Name == "TestUnit")), Times.Once);
			Assert.Equal(1, result);
		}
		[Fact]
		public void Get_GetAllUnitsForSelectList_ShouldGetAllList()
		{
			//Arrange
			var units = new List<Unit>
			{
				new() { Id = 1, Name="TestU1" },
				new() { Id = 2,  Name="TestU2"}
			};
			var unitVms = new List<UnitForListVm>
			{
				new () { Id = 1, Name="TestU1" },
				new () { Id = 2, Name="TestU2" }
			};			
			_unitRepoMock.Setup(repo => repo.GetAllUnits())	.Returns(units.AsQueryable);		
			_mapperMock.Setup(map => map.Map<List<UnitForListVm>>(units)).Returns(unitVms);			
			//Act
			var result = _unitService.GetUnitsForSelectList();
			//Assert
			Assert.NotNull(result);
			Assert.IsType<List<SelectListItem>>(result);
			Assert.Equal(2, result.Count);
			Assert.Contains(result, c => c.Text == "TestU1" && c.Value == "1");
			Assert.Contains(result, c => c.Text == "TestU2" && c.Value == "2");
		}
		[Fact]
		public void GetOrAdd_GetOrAddUnit_ShouldAddNewUnit()
		{
			//Assert
			var newUnitVm = new IngredientForNewRecipeVm
			{
				IngredientUnit = 0,
				NewIngredientUnit = "kg"
			};
			_unitRepoMock.Setup(i => i.ExistingUnit(It.IsAny<string>())).Returns((Unit)null);
			_unitRepoMock.Setup(u => u.AddUnit(It.IsAny<Unit>())).Returns(1);			
			//Act
			var result = _unitService.GetOrAddUnit(newUnitVm);
			//Assert
			Assert.NotEqual(0, result);
			Assert.Equal(1, result);
			_unitRepoMock.Verify(i => i.AddUnit(It.IsAny<Unit>()), Times.Once);
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
			_unitRepoMock.Setup(u => u.ExistingUnit((It.IsAny<string>()))).Returns(newUnit);			
			//Act
			var result = _unitService.GetOrAddUnit(newUnitVm);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(2, result);
		}
	}
}

