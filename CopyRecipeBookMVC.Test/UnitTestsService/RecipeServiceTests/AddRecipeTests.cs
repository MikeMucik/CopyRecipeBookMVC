using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.RecipeIngredient;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Application.Test.UnitTestsService.RecipeServiceTests
{
	public class AddRecipeTests
	{
		private readonly Mock<IRecipeRepository> _recipeRepoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Mock<IRecipeIngredientService> _recipeIngredientServiceMock;
		private readonly RecipeService _recipeService;
		public AddRecipeTests()
		{
			_recipeRepoMock = new Mock<IRecipeRepository>();
			_mapperMock = new Mock<IMapper>();
			_recipeIngredientServiceMock = new Mock<IRecipeIngredientService>();
			_recipeService = new RecipeService(_recipeRepoMock.Object,
				_mapperMock.Object, _recipeIngredientServiceMock.Object);
		}
		[Fact]
		public void Add_AddRecipe_ShouldAddRecipeToCollection()
		{
			//Arrange
			var newRecipeVm = new NewRecipeVm
			{
				Id = 1,
				Name = "Test Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Ingredients = new List<IngredientForNewRecipeVm> {
						new IngredientForNewRecipeVm {IngredientName = 1, IngredientUnit = 1, Quantity = 100 },
						new IngredientForNewRecipeVm {IngredientName = 2, IngredientUnit = 2, Quantity = 200 }
					},
				Description = ""
			};
			var recipe = new Recipe
			{
				Id = 1,
				Name = "Test Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
			};
			_recipeRepoMock.Setup(repo => repo.AddRecipe(It.IsAny<Recipe>())).Returns(1);
			_mapperMock.Setup(mapper => mapper.Map<Recipe>(It.IsAny<NewRecipeVm>()));
			//Act
			var result = _recipeService.AddRecipe(newRecipeVm);
			// Assert
			Assert.Equal(1, result);
			_recipeRepoMock.Verify(repo => repo.AddRecipe(It.IsAny<Recipe>()), Times.Once);
			_recipeIngredientServiceMock.Verify(service => service.AddCompleteIngredients(It.IsAny<RecipeIngredient>()), Times.Exactly(2));
			_recipeIngredientServiceMock.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
			   ri.RecipeId == 1 && ri.IngredientId == 1 && ri.UnitId == 1 && ri.Quantity == 100)), Times.Once);
			_recipeIngredientServiceMock.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
				ri.RecipeId == 1 && ri.IngredientId == 2 && ri.UnitId == 2 && ri.Quantity == 200)), Times.Once);
		}

		//[Fact]
		//public void Add_AddRecipe_ShouldNotAddRecipeToCollection()
		//{
		//	//Arrange
		//	var newRecipeVm = new NewRecipeVm { 
		//	//{
		//	//	Id = 0,
		//	//	Name = "Test Recipe Vm",
		//	//	CategoryId = 1,
		//	//	DifficultyId = 1,
		//	//	TimeId = 1,
		//	Ingredients = new List<IngredientForNewRecipeVm> {
		//				new IngredientForNewRecipeVm {IngredientName = 1, IngredientUnit = 1, Quantity = 100 },
		//				new IngredientForNewRecipeVm {IngredientName = 2, IngredientUnit = 2, Quantity = 200 }
		//			}};
		//	//	Description = ""
		//	//};
		//	//var recipe = new Recipe
		//	//{
		//	//	Id = 1,
		//	//	Name = "Test Recipe Vm",
		//	//	CategoryId = 1,
		//	//	DifficultyId = 1,
		//	//	TimeId = 1,
		//	//};
		//	_recipeRepoMock.Setup(repo => repo.AddRecipe(It.IsAny<Recipe>())).Returns(1);
		//	_mapperMock.Setup(mapper => mapper.Map<Recipe>(It.IsAny<NewRecipeVm>()));
		//	//Act
		//	var result = _recipeService.AddRecipe(newRecipeVm);
		//	// Assert
		//	Assert.Equal(1, result);
		//	_recipeRepoMock.Verify(repo => repo.AddRecipe(It.IsAny<Recipe>()), Times.Once);
		//	_recipeIngredientServiceMock.Verify(service => service.AddCompleteIngredients(It.IsAny<RecipeIngredient>()), Times.Exactly(2));
		//	_recipeIngredientServiceMock.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
		//	   ri.RecipeId == 1 && ri.IngredientId == 1 && ri.UnitId == 1 && ri.Quantity == 100)), Times.Once);
		//	_recipeIngredientServiceMock.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
		//		ri.RecipeId == 1 && ri.IngredientId == 2 && ri.UnitId == 2 && ri.Quantity == 200)), Times.Once);
		//}
	}
}
