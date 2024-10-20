using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Difficulty;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Test.UnitTests
{
    public class DifficultyServiceTests
    {
        private readonly Mock<IDifficultyRepository> _difficultyRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DifficultyService _difficultyService;
        public DifficultyServiceTests()
        {
            _difficultyRepoMock = new Mock<IDifficultyRepository>();
            _mapperMock = new Mock<IMapper>();
            _difficultyService = new DifficultyService(_difficultyRepoMock.Object, _mapperMock.Object);
        }
        [Fact]
        public void Get_GetListDifficulyForList_ShouldGetAllList()
        {
            //Arrange
            var difficulties = new List<Difficulty>
            {
                new Difficulty { Id = 1, Name = "TestD1" },
                new Difficulty { Id = 2, Name = "TestD2" }
            };
            var difficultyVms = new List<DifficultyForListVm>
            {
                new DifficultyForListVm { Id = 1, Name = "TestD1" },
                new DifficultyForListVm { Id = 2, Name = "TestD2" }
            };
           _difficultyRepoMock.Setup(repo => repo.GetAllDifficulties()) .Returns(difficulties);
            _mapperMock.Setup(mapper => mapper.Map<List<DifficultyForListVm>>(difficulties))
                .Returns(difficultyVms);
            //Act
            var result =_difficultyService.GetListDifficultyForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListDifficultyForListVm>(result);
            Assert.Equal(2, result.Difficulties.Count);
            Assert.Contains(result.Difficulties, d => d.Name == "TestD1");
            Assert.Contains(result.Difficulties, d => d.Name == "TestD2");
        }
    }
}
