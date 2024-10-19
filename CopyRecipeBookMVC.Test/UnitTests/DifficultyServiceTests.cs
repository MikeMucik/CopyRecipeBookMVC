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
            var mockRepo = new Mock<IDifficultyRepository>();
            mockRepo
                .Setup(repo => repo.GetAllDifficulties())
                .Returns(difficulties);

            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(mapper => mapper.Map<List<DifficultyForListVm>>(difficulties))
                .Returns(difficultyVms);

            var mockService = new DifficultyService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetListDifficultyForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListDifficultyForListVm>(result);
            Assert.Equal(2, result.Difficulties.Count);
            Assert.Contains(result.Difficulties, d => d.Name == "TestD1");
            Assert.Contains(result.Difficulties, d => d.Name == "TestD2");
        }
    }
}
