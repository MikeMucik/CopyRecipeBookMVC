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
	public class RecipeServiceTests
	{
		private readonly Mock<IRecipeRepository> _recipeRepoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Mock<IRecipeIngredientService> _recipeIngredientServiceMock;
		private readonly RecipeService _recipeService;

		public RecipeServiceTests()
		{
			_recipeRepoMock = new Mock<IRecipeRepository>();
			_mapperMock = new Mock<IMapper>();
			_recipeIngredientServiceMock = new Mock<IRecipeIngredientService>();
			_recipeService = new RecipeService(_recipeRepoMock.Object,
				_mapperMock.Object, _recipeIngredientServiceMock.Object);
		}
		[Fact]
		public void Find_TryAddRecipe_ShouldFindExistingRecipeDifSize()
		{
			//Arrange
			var newRecipe = new NewRecipeVm
			{
				Name = "test"
			};
			var existingRecipe = new Recipe
			{
				Id = 1,
				Name = "Test"
			};
			_recipeRepoMock.Setup(repo => repo.FindByName(newRecipe.Name)).Returns(existingRecipe);
			//Act
			var result = _recipeService.TryAddRecipe(newRecipe);
			//Assert			
			Assert.NotNull(result);
			Assert.Equal(1, result);
		}
		[Fact]
		public void Find_TryAddRecipe_ShouldNotFindExistingRecipe()
		{
			//Arrange
			var newRecipe = new NewRecipeVm
			{
				Name = "Test1"
			};
			var existingRecipe = new Recipe
			{
				Id = 1,
				Name = "NotTest"
			};
			//	  .Returns(null);
			//Act
			var result = _recipeService.TryAddRecipe(newRecipe);
			//Assert			
			Assert.Null(result);
		}
		[Fact]
		public void Return_GetAllRecipesForList_ShouldReturnAllList()
		{
			//Arrange
			var recipeList = new List<Recipe>
			{
				new Recipe {Id = 1, Name= "Test1", Category = new Category{Name = "Śniadanie" }, Difficulty=new Difficulty{Name="Łatwy" }, Time = new Time{Amount = 5, Unit = "m" } },
				new Recipe {Id = 2, Name= "Test2", Category = new Category{Name = "Obiad" }, Difficulty=new Difficulty{Name="Trudny" }, Time = new Time{Amount = 1, Unit = "h" } }
			};
			_recipeRepoMock.Setup(repo => repo.GetAllRecipes()).Returns(recipeList.AsQueryable());
			_mapperMock.Setup(mapper => mapper.ConfigurationProvider)
				.Returns(new MapperConfiguration(mc =>
				{
					mc.CreateMap<Recipe, RecipeListForVm>()
					.ForMember(r => r.Category, opt => opt.MapFrom(s => s.Category.Name))
					.ForMember(r => r.Difficulty, opt => opt.MapFrom(s => s.Difficulty.Name))
					.ForMember(r => r.Time, opt => opt.MapFrom(s => s.Time.Amount + " " + s.Time.Unit));
				}));
			int pageSize = 3;
			int pageNumber = 1;
			string searchString = "";
			//Act
			var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
		}
		[Fact]
		public void Return_GetAllRecipesForList_ShouldReturnFilteredRecipes()
		{
			//Arrange
			var recipeList = new List<Recipe>
			{
				new Recipe {Id = 1, Name= "Test1", Category = new Category{Name = "Śniadanie" }, Difficulty=new Difficulty{Name="Łatwy" }, Time = new Time{Amount = 5, Unit = "m" } },
				new Recipe {Id = 2, Name= "Test2", Category = new Category{Name = "Obiad" }, Difficulty=new Difficulty{Name="Trudny" }, Time = new Time{Amount = 1, Unit = "h" } }
			};
			_recipeRepoMock.Setup(repo => repo.GetAllRecipes()).Returns(recipeList.AsQueryable());
			_mapperMock.Setup(mapper => mapper.ConfigurationProvider)
			.Returns(new MapperConfiguration(mc =>
			{
				mc.CreateMap<Recipe, RecipeListForVm>()
				.ForMember(r => r.Category, opt => opt.MapFrom(s => s.Category.Name))
				.ForMember(r => r.Difficulty, opt => opt.MapFrom(s => s.Difficulty.Name))
				.ForMember(r => r.Time, opt => opt.MapFrom(s => s.Time.Amount + " " + s.Time.Unit));
			}));
			int pageSize = 3;
			int pageNumber = 1;
			string searchString = "Test1";
			//Act
			var result = _recipeService.GetAllRecipesForList(pageSize, pageNumber, searchString);
			//Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Count);
			Assert.Equal("Test1", result.Recipes.First().Name);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);
			Assert.Equal(searchString, result.SearchString);
		}
		[Fact]
		public void Return_GetRecipesByCategory_ShouldReturnFilteredRecipes()
		{
			//Arrange
			int pageSize = 3;
			int pageNumber = 1;
			int categoryId = 1;
			var recipeList = new List<Recipe>
			{
				new Recipe {Id = 1, Name= "Test1", Category = new Category{Id = 1 , Name ="Śniadanie" }, Difficulty=new Difficulty{Name="Łatwy" }, Time = new Time{Amount = 5, Unit = "m" } },
				new Recipe {Id = 2, Name= "Test2", Category = new Category{Id = 2,  Name = "Obiad" }, Difficulty=new Difficulty{Name="Trudny" }, Time = new Time{Amount = 1, Unit = "h" } }
			};
			_recipeRepoMock.Setup(repo => repo.GetRecipesByCategory(categoryId)).Returns(recipeList.AsQueryable());
			_mapperMock.Setup(mapper => mapper.ConfigurationProvider)
			.Returns(new MapperConfiguration(mc =>
			{
				mc.CreateMap<Recipe, RecipeListForVm>()
				.ForMember(r => r.Category, opt => opt.MapFrom(s => s.Category.Name))
				.ForMember(r => r.Difficulty, opt => opt.MapFrom(s => s.Difficulty.Name))
				.ForMember(r => r.Time, opt => opt.MapFrom(s => s.Time.Amount + " " + s.Time.Unit));

			}));

			//Act
			var result = _recipeService.GetRecipesByCategory(pageSize, pageNumber, categoryId);
			//Assert
			Assert.NotNull(result);
			//Assert.Equal(1, result.Count);
			Assert.Equal("Test1", result.RecipesByCategory.First().Name);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);
			Assert.Equal(categoryId, result.CategoryId);
		}
		[Fact]
		public void Return_GetRecipesByDifficulty_ShouldReturnFilteredRecipes()
		{
			//Arrange
			int pageSize = 3;
			int pageNumber = 1;
			int difficultyId = 1;
			var recipeList = new List<Recipe>
			{
				new Recipe {Id = 1, Name= "Test1", Category = new Category{ Name ="Śniadanie" }, Difficulty=new Difficulty{Id = 1 ,Name="Łatwy" }, Time = new Time{Amount = 5, Unit = "m" } },
				new Recipe {Id = 2, Name= "Test2", Category = new Category{ Name = "Obiad" }, Difficulty=new Difficulty{Id = 2, Name="Trudny" }, Time = new Time{Amount = 1, Unit = "h" } }
			};
			_recipeRepoMock.Setup(repo => repo.GetRecipesByDifficulty(difficultyId)).Returns(recipeList.AsQueryable());
			_mapperMock.Setup(mapper => mapper.ConfigurationProvider)
			.Returns(new MapperConfiguration(mc =>
			{
				mc.CreateMap<Recipe, RecipeListForVm>()
				.ForMember(r => r.Category, opt => opt.MapFrom(s => s.Category.Name))
				.ForMember(r => r.Difficulty, opt => opt.MapFrom(s => s.Difficulty.Name))
				.ForMember(r => r.Time, opt => opt.MapFrom(s => s.Time.Amount + " " + s.Time.Unit));

			}));

			//Act
			var result = _recipeService.GetRecipesByDifficulty(pageSize, pageNumber, difficultyId);
			//Assert
			Assert.NotNull(result);
			//Assert.Equal(1, result.Count);
			Assert.Equal("Test1", result.RecipesByDifficulty.First().Name);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);
			Assert.Equal(difficultyId, result.DifficultyId);
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
				Time = "10" + " " + "minut",

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
			_recipeRepoMock
				.Setup(repo => repo.GetRecipeById(recipeIdToShow))
				.Returns(recipe);
			_mapperMock
				.Setup(mapper => mapper.Map<RecipeDetailsVm>(It.IsAny<Recipe>()))
				.Returns(recipeDetailsVm);
			//Act
			var result = _recipeService.GetRecipe(recipeIdToShow);
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
		public void Take_GetRecipeToEdit_ShouldTakeRecipeByIdAndReturnInNewRecipeVm()
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
			_recipeRepoMock.Setup(repo => repo.GetRecipeById(recipeIdToEdit)).Returns(recipe);
			_mapperMock.Setup(mapper => mapper.Map<NewRecipeVm>(It.IsAny<Recipe>()))
				.Returns(newRecipeVm);
			//Act
			var result = _recipeService.GetRecipeToEdit(recipeIdToEdit);
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
				Assert.Equal(newRecipeVm.Ingredients[i].IngredientName, result.Ingredients[i].IngredientName);
				Assert.Equal(newRecipeVm.Ingredients[i].IngredientUnit, result.Ingredients[i].IngredientUnit);
				Assert.Equal(newRecipeVm.Ingredients[i].Quantity, result.Ingredients[i].Quantity);
			}
		}
		[Fact]//do QueryCollection
		public void ReturnByIngredients_GetRecipesByIngredients_ReturnListRecipes()
		{
			//Arrange
			int pageSize = 3;
			int pageNumber = 1;
			List<int> ingredientIds = new List<int> { 1, 2 };
			var recipeList = new List<Recipe>
			{
				 new Recipe {Id = 1, Name= "Test1",
					Category = new Category{ Name ="Śniadanie" },
					Difficulty=new Difficulty{Id = 1 ,Name="Łatwy" },
					Time = new Time{Amount = 5, Unit = "m" },
					RecipeIngredient = new List<RecipeIngredient>
					{
						new RecipeIngredient{RecipeId = 1, IngredientId =1, UnitId =1, Quantity=1}
					}
				},
				new Recipe {Id = 2, Name= "Test2",
					Category = new Category{ Name = "Obiad" },
					Difficulty=new Difficulty{Id = 2, Name="Trudny" },
					Time = new Time{Amount = 1, Unit = "h" },
				RecipeIngredient = new List<RecipeIngredient>
					{
						new RecipeIngredient{RecipeId =2, IngredientId =2, UnitId =2, Quantity=2}
					}
				}
			};
			_recipeRepoMock.Setup(repo => repo.GetRecipesByIngredients(ingredientIds.ToList())).Returns(recipeList.AsQueryable());
			_mapperMock.Setup(mapper => mapper.ConfigurationProvider)
			.Returns(new MapperConfiguration(mc =>
			{
				mc.CreateMap<Recipe, RecipeListForVm>()
				.ForMember(r => r.Category, opt => opt.MapFrom(s => s.Category.Name))
				.ForMember(r => r.Difficulty, opt => opt.MapFrom(s => s.Difficulty.Name))
				.ForMember(r => r.Time, opt => opt.MapFrom(s => s.Time.Amount + " " + s.Time.Unit));
			}));

			//Act
			var result = _recipeService.GetRecipesByIngredients(pageSize, pageNumber, ingredientIds);
			//Assert
			Assert.NotNull(result);
			//Assert.Equal(1, result.Count);
			Assert.Equal("Test1", result.RecipesByIngredients.First().Name);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(pageNumber, result.CurrentPage);
			Assert.Equal(ingredientIds, result.IngredientIds);
			_recipeRepoMock.Verify(repo => repo.GetRecipesByIngredients(It.IsAny<List<int>>()), Times.Once);
			_mapperMock.Verify(mapper => mapper.ConfigurationProvider, Times.Once);
		}

	}
}
