using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Services;
using CopyRecipeBookMVC.Application.ViewModels.Time;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Domain.Model;
using Moq;

namespace CopyRecipeBookMVC.Test.UnitTests
{
    public class TimeServiceTests
    {
        [Fact]
        public void AddOrGetTime_AddTime_ShouldAddTimeToRecipesWhenTimeExisting()
        {
            //Arrange
            var existingTime = new Time
            {
                Id = 1,
                Amount = 1,
                Unit = "m"
            };
            var newTimeVm = new Application.ViewModels.Recipe.NewRecipeVm
            {
                TimeAmount = 1,
                TimeUnit = "m"
            };

            var mockRepo = new Mock<ITimeRepositoy>();
            mockRepo
                .Setup(repo => repo.ExistingTime(newTimeVm.TimeAmount, newTimeVm.TimeUnit))
                .Returns(existingTime);
            var mockMapper = new Mock<IMapper>();

            var mockService = new TimeService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.AddTime(newTimeVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            mockRepo.Verify(repo => repo.ExistingTime(newTimeVm.TimeAmount, newTimeVm.TimeUnit), Times.Once);
            mockRepo.Verify(repo => repo.AddTime(It.IsAny<Time>()), Times.Never);
        }

        [Fact]
        public void AddOrGetTime_AddTime_ShouldAddTimeToCollectionWhenTimeNotExisting()
        {

            var newTimeVm = new Application.ViewModels.Recipe.NewRecipeVm
            {
                TimeAmount = 1,
                TimeUnit = "m"
            };

            var mockRepo = new Mock<ITimeRepositoy>();
            mockRepo
                .Setup(repo => repo.AddTime(It.IsAny<Time>()))
                .Returns(1);
            mockRepo
                .Setup(repo => repo.ExistingTime(newTimeVm.TimeAmount, newTimeVm.TimeUnit))
                .Returns((Time)null);

            var mockMapper = new Mock<IMapper>();

            var mockService = new TimeService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.AddTime(newTimeVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            mockRepo.Verify(repo => repo.AddTime(It.IsAny<Time>()), Times.Once);

        }

        [Fact]
        public void Get_GetListTimeForList()
        {
            //Arrange
            var times = new List<Time>
            {
                new Time {Id = 1, Amount = 1, Unit ="h"},
                new Time {Id = 2, Amount = 1, Unit ="m"}
            };
            var timesVms = new List<TimeForListVm>
            {
                new TimeForListVm { Id = 1, Amount = 1, Unit = "h" },
                new TimeForListVm { Id = 2, Amount = 1, Unit = "m" }
            };

            var mockRepo = new Mock<ITimeRepositoy>();
            mockRepo
                .Setup(repo => repo.GetAllTimes())
                .Returns(times.AsQueryable);

            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(mapper => mapper.Map<List<TimeForListVm>>(times))
                .Returns(timesVms);

            var mockService = new TimeService(mockRepo.Object, mockMapper.Object);
            //Act
            var result = mockService.GetListTimeForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListTimeForListVm>(result);
            Assert.Equal(2, result.Times.Count);
            Assert.Contains(result.Times, t => t.Unit == "h");
            Assert.Contains(result.Times, t => t.Unit == "m");
        }
    }
}
