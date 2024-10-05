using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Interfaces;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using CopyRecipeBookMVC.Infrastructure;
using Moq;
using Moq.EntityFrameworkCore;


namespace CopyRecipeBookMVC.Test
{
	public class RecipeServiceTests
	{
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
						new IngredientForNewRecipeVm {Name = 1, Unit = 1, Quantity = 100 },
						new IngredientForNewRecipeVm {Name = 2, Unit = 2, Quantity = 200 }
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

			var mockRecipeRepository = new Mock<IRecipeRepository>();
			mockRecipeRepository
				.Setup(repo => repo.AddRecipe(It.IsAny<Recipe>()))
				.Returns(1);
			var mockIngredientService = new Mock<IIngredientService>();
			var mockMapper = new Mock<IMapper>();
			mockMapper
				.Setup(mapper => mapper.Map<Recipe>(It.IsAny<NewRecipeVm>()));
			var mockService = new RecipeService(mockRecipeRepository.Object, mockMapper.Object, mockIngredientService.Object);

			//Act
			var result = mockService.AddRecipe(newRecipeVm);

			// Assert

			mockRecipeRepository.Verify(repo => repo.AddRecipe(It.IsAny<Recipe>()), Times.Once);
			mockIngredientService.Verify(service => service.AddCompleteIngredients(It.IsAny<RecipeIngredient>()), Times.Exactly(2));


			mockIngredientService.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
			   ri.RecipeId == 1 && ri.IngredientId == 1 && ri.UnitId == 1 && ri.Quantity == 100)), Times.Once);

			mockIngredientService.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
				ri.RecipeId == 1 && ri.IngredientId == 2 && ri.UnitId == 2 && ri.Quantity == 200)), Times.Once);

			Assert.Equal(1, result);
		}
		
		[Fact]
		public void Take_GetRecipe_ShouldTakeReacipeByIdAndReturnInRecipeDetailsVm()
		{
			//Arrange
			var recipeDetailsVm = new RecipeDetailsVm
			{
				Id = 1,
				Name = "Test Recipe Vm",
				Category = "Śniadanie",
				Difficulty = "Bardzo łatwy",
				Time = "10"+" "+"minut",
				
				Ingredients = new List<IngredientForRecipeVm> {
						new IngredientForRecipeVm {Name = "Jajko", Unit = "Sztuka", Quantity = 100 },
						new IngredientForRecipeVm {Name = "Boczek", Unit = "Gram", Quantity = 200 }
					},
				Description = ""
			};
			var recipeIdToShow = 1;

			var recipe = new Recipe
			{
				Id = 1,
				Name = "Test Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				RecipeIngredient = new List<RecipeIngredient>
				 {
						new RecipeIngredient {RecipeId = 1, IngredientId= 1, UnitId = 1, Quantity = 100 },
						new RecipeIngredient {RecipeId = 1, IngredientId= 2, UnitId = 2, Quantity = 200 },
					},
				Description = ""
			};
			var mockRecipeRepository = new Mock<IRecipeRepository>();
			mockRecipeRepository
				.Setup(repo => repo.GetRecipeById(recipeIdToShow))
				.Returns(recipe);
			var mockIngredientService = new Mock<IIngredientService>();
			var mockMapper = new Mock<IMapper>();
			mockMapper
				.Setup(mapper => mapper.Map<RecipeDetailsVm>(It.IsAny<Recipe>()))
				.Returns(recipeDetailsVm);

			var mockService = new RecipeService(mockRecipeRepository.Object, mockMapper.Object, mockIngredientService.Object);
			//Act
			var result = mockService.GetRecipe(recipeIdToShow);
			//Assert
			Assert.Equal(recipeDetailsVm.Id, result.Id);
			Assert.Equal(recipeDetailsVm.Name, result.Name);
			Assert.Equal(recipeDetailsVm.Category, result.Category);
			Assert.Equal(recipeDetailsVm.Difficulty, result.Difficulty);
			Assert.Equal(recipeDetailsVm.Time, result.Time);
			Assert.Equal(recipeDetailsVm.Description, result.Description);

			Assert.Equal(recipeDetailsVm.Ingredients.Count, result.Ingredients.Count);
			for (int i = 0; i < recipeDetailsVm.Ingredients.Count; i++)
			{
				Assert.Equal(recipeDetailsVm.Ingredients[i].Name, result.Ingredients[i].Name);
				Assert.Equal(recipeDetailsVm.Ingredients[i].Unit, result.Ingredients[i].Unit);
				Assert.Equal(recipeDetailsVm.Ingredients[i].Quantity, result.Ingredients[i].Quantity);
			}
		}


		[Fact]
		public void Take_GetRecipeEdit_ShouldTakeRecipeByIdAndReturnInNewRecipeVm()
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
						new IngredientForNewRecipeVm {Name = 1, Unit = 1, Quantity = 100 },
						new IngredientForNewRecipeVm {Name = 2, Unit = 2, Quantity = 200 }
					},
				Description = ""
			};
			var recipeIdToEdit = 1;

			var recipe = new Recipe
			{
				Id = 1,
				Name = "Test Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				RecipeIngredient = new List<RecipeIngredient>
				 {
						new RecipeIngredient {RecipeId = 1, IngredientId= 1, UnitId = 1, Quantity = 100 },
						new RecipeIngredient {RecipeId = 1, IngredientId= 2, UnitId = 2, Quantity = 200 },
					},
				Description = ""
			};
			var mockRecipeRepository = new Mock<IRecipeRepository>();
			mockRecipeRepository
				.Setup(repo => repo.GetRecipeById(recipeIdToEdit))
				.Returns(recipe);
			var mockIngredientService = new Mock<IIngredientService>();
			var mockMapper = new Mock<IMapper>();
			mockMapper
				.Setup(mapper => mapper.Map<NewRecipeVm>(It.IsAny<Recipe>()))
				.Returns(newRecipeVm);

			var mockService = new RecipeService(mockRecipeRepository.Object, mockMapper.Object, mockIngredientService.Object);
			//Act
			var result = mockService.GetRecipeToEdit(recipeIdToEdit);
			//Assert
			Assert.Equal(newRecipeVm.Id, result.Id);
			Assert.Equal(newRecipeVm.Name, result.Name);
			Assert.Equal(newRecipeVm.CategoryId, result.CategoryId);
			Assert.Equal(newRecipeVm.DifficultyId, result.DifficultyId);
			Assert.Equal(newRecipeVm.TimeId, result.TimeId);
			Assert.Equal(newRecipeVm.Description, result.Description);

			Assert.Equal(newRecipeVm.Ingredients.Count, result.Ingredients.Count);
			for (int i = 0; i < newRecipeVm.Ingredients.Count; i++)
			{
				Assert.Equal(newRecipeVm.Ingredients[i].Name, result.Ingredients[i].Name);
				Assert.Equal(newRecipeVm.Ingredients[i].Unit, result.Ingredients[i].Unit);
				Assert.Equal(newRecipeVm.Ingredients[i].Quantity, result.Ingredients[i].Quantity);
			}
		}


		[Fact]
		public void Edit_UpdateRecipe_ShouldEditRecipeInCollection()
		{
			//Arrange
			var recipeId = 1;

			var updatedRecipeVm = new NewRecipeVm
			{
				Id = recipeId,
				Name = "Updated Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Ingredients = new List<IngredientForNewRecipeVm> {
						new IngredientForNewRecipeVm {Name = 1, Unit = 1, Quantity = 100 },
						new IngredientForNewRecipeVm {Name = 2, Unit = 2, Quantity = 200 }
					},
				Description = ""
			};

			var existRecipeVm = new NewRecipeVm
			{
				Id = recipeId,
				Name = "Exist Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				Ingredients = new List<IngredientForNewRecipeVm> {
						new IngredientForNewRecipeVm {Name = 1, Unit = 1, Quantity = 100 },
						new IngredientForNewRecipeVm {Name = 2, Unit = 2, Quantity = 200 }
					},
				Description = ""
			};

			var existingRecipe = new Recipe
			{
				Id = recipeId,
				Name = "Original Recipe Vm",
				CategoryId = 1,
				DifficultyId = 1,
				TimeId = 1,
				RecipeIngredient =				
				new List<RecipeIngredient>
				{
					new RecipeIngredient { RecipeId = recipeId, IngredientId = 1, UnitId = 1, Quantity = 100 },
					new RecipeIngredient { RecipeId = recipeId, IngredientId = 2, UnitId = 2, Quantity = 200 }
				},
				Description = ""
			};

			var mockRecipeRepository = new Mock<IRecipeRepository>();
			var mockIngredientService = new Mock<IIngredientService>();
			var mockMapper = new Mock<IMapper>();
				mockMapper
				.Setup(map=>map.Map<NewRecipeVm>(It.IsAny<Recipe>()))
				.Returns(existRecipeVm);
			var mockService = new RecipeService(mockRecipeRepository.Object, mockMapper.Object, mockIngredientService.Object);


			//Act
			mockService.UpdateRecipe(updatedRecipeVm);

			//Assert
			//mockIngredientService.Verify(service => service.DeleteCompleteIngredients(recipeId), Times.Once); // Usunięcie starych składników
			mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Once); // Aktualizacja przepisu
			//mockIngredientService.Verify(service => service.AddCompleteIngredients(It.IsAny<RecipeIngredient>()), Times.Exactly(2)); // Dodanie nowych składników

			//Assert.Equal(newRecipeVm.Id, result.Id);
			//Assert.Equal(newRecipeVm.Name, result.Name);
			//Assert.Equal(newRecipeVm.CategoryId, result.CategoryId);
			//Assert.Equal(newRecipeVm.DifficultyId, result.DifficultyId);
			//Assert.Equal(newRecipeVm.TimeId, result.TimeId);
			//Assert.Equal(newRecipeVm.Description, result.Description);

			//// Dodatkowo można sprawdzić konkretne wartości składników
			//mockIngredientService.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
			//	ri.RecipeId == recipeId && ri.IngredientId == 1 && ri.UnitId == 1 && ri.Quantity == 150)), Times.Once);

			//mockIngredientService.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
			//	ri.RecipeId == recipeId && ri.IngredientId == 2 && ri.UnitId == 2 && ri.Quantity == 250)), Times.Once);


		}
		//[Fact]
		//public void UpdateRecipe_ShouldUpdateRecipeAndManageIngredients()
		//{
		//    // Arrange
		//    var recipeId = 1;

		//    // Obiekt do aktualizacji (z nowymi składnikami)
		//    var updatedRecipeVm = new NewRecipeVm
		//    {
		//        Id = recipeId,
		//        Name = "Updated Recipe",
		//        Ingredients = new List<IngredientForRecipeVm>
		//        {
		//            new IngredientForRecipeVm { Name = 1, Unit = 1, Quantity = 150 },
		//            new IngredientForRecipeVm { Name = 2, Unit = 2, Quantity = 250 }
		//        }
		//    };

		//    // Obiekt przepisu pobranego z bazy przed aktualizacją (obecny stan przepisu)
		//    var existingRecipe = new Recipe
		//    {
		//        Id = recipeId,
		//        Name = "Original Recipe",
		//        Ingredients = new List<RecipeIngredient>
		//        {
		//            new RecipeIngredient { RecipeId = recipeId, IngredientId = 1, UnitId = 1, Quantity = 100 },
		//            new RecipeIngredient { RecipeId = recipeId, IngredientId = 2, UnitId = 2, Quantity = 200 }
		//        }
		//    };

		//    // Mockowanie IMapper (Mapowanie z NewRecipeVm na Recipe)
		//    var mockMapper = new Mock<IMapper>();
		//    mockMapper.Setup(m => m.Map<Recipe>(It.IsAny<NewRecipeVm>())).Returns(new Recipe { Id = recipeId, Name = "Updated Recipe" });

		//    // Mockowanie IRecipeRepository (pobranie przepisu i jego aktualizacja)
		//    var mockRecipeRepo = new Mock<IRecipeRepository>();
		//    mockRecipeRepo.Setup(repo => repo.GetRecipeById(recipeId)).Returns(existingRecipe); // Zwrócenie istniejącego przepisu
		//    mockRecipeRepo.Setup(repo => repo.UpdateRecipe(It.IsAny<Recipe>())); // Aktualizacja przepisu

		//    // Mockowanie IIngredientService (usuwanie i dodawanie składników)
		//    var mockIngredientService = new Mock<IIngredientService>();
		//    mockIngredientService.Setup(service => service.DeleteCompleteIngredients(recipeId)); // Usuwanie starych składników
		//    mockIngredientService.Setup(service => service.AddCompleteIngredients(It.IsAny<RecipeIngredient>()));

		//    // Tworzenie instancji RecipeService z zamockowanymi zależnościami
		//    var recipeService = new RecipeService(mockRecipeRepo.Object, mockMapper.Object, mockIngredientService.Object);

		//    // Act - wywołanie metody UpdateRecipe
		//    recipeService.UpdateRecipe(updatedRecipeVm);

		//    // Assert - weryfikacja, czy składniki zostały usunięte, przepis zaktualizowany i składniki dodane
		//    mockIngredientService.Verify(service => service.DeleteCompleteIngredients(recipeId), Times.Once); // Usunięcie starych składników
		//    mockRecipeRepo.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Once); // Aktualizacja przepisu
		//    mockIngredientService.Verify(service => service.AddCompleteIngredients(It.IsAny<RecipeIngredient>()), Times.Exactly(2)); // Dodanie nowych składników

		//    // Dodatkowo można sprawdzić konkretne wartości składników
		//    mockIngredientService.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
		//        ri.RecipeId == recipeId && ri.IngredientId == 1 && ri.UnitId == 1 && ri.Quantity == 150)), Times.Once);

		//    mockIngredientService.Verify(service => service.AddCompleteIngredients(It.Is<RecipeIngredient>(ri =>
		//        ri.RecipeId == recipeId && ri.IngredientId == 2 && ri.UnitId == 2 && ri.Quantity == 250)), Times.Once);
		//}

	}
}
