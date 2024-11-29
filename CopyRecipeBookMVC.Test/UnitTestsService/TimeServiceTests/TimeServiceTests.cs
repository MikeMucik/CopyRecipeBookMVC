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

namespace CopyRecipeBookMVC.Application.Test.UnitTestsService.TimeServiceTests
{
    public class TimeServiceTests
    {
        private readonly Mock<ITimeRepositoy> _timeRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TimeService _timeService;
        public TimeServiceTests()
        {
            _timeRepoMock = new Mock<ITimeRepositoy>();
            _mapperMock = new Mock<IMapper>();
            _timeService = new TimeService(_timeRepoMock.Object, _mapperMock.Object);
        }
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
            var newTimeVm = new ViewModels.Recipe.NewRecipeVm
            {
                TimeAmount = 1,
                TimeUnit = "m"
            };
            _timeRepoMock.Setup(repo => repo.ExistingTime(newTimeVm.TimeAmount, newTimeVm.TimeUnit))
                .Returns(existingTime);
            //Act
            var result = _timeService.AddTime(newTimeVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            _timeRepoMock.Verify(repo => repo.ExistingTime(newTimeVm.TimeAmount, newTimeVm.TimeUnit), Times.Once);
            _timeRepoMock.Verify(repo => repo.AddTime(It.IsAny<Time>()), Times.Never);
        }
        [Fact]
        public void AddOrGetTime_AddTime_ShouldAddTimeToCollectionWhenTimeNotExisting()
        {
            var newTimeVm = new ViewModels.Recipe.NewRecipeVm
            {
                TimeAmount = 1,
                TimeUnit = "m"
            };
            _timeRepoMock.Setup(repo => repo.AddTime(It.IsAny<Time>())).Returns(1);
            _timeRepoMock.Setup(repo => repo.ExistingTime(newTimeVm.TimeAmount, newTimeVm.TimeUnit))
                .Returns((Time)null);
            //Act
            var result = _timeService.AddTime(newTimeVm);
            //Assert
            Assert.NotEqual(0, result);
            Assert.Equal(1, result);
            _timeRepoMock.Verify(repo => repo.AddTime(It.IsAny<Time>()), Times.Once);
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
            _timeRepoMock.Setup(repo => repo.GetAllTimes()).Returns(times.AsQueryable);
            _mapperMock.Setup(mapper => mapper.Map<List<TimeForListVm>>(times))
                .Returns(timesVms);
            //Act
            var result = _timeService.GetListTimeForList();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ListTimeForListVm>(result);
            Assert.Equal(2, result.Times.Count);
            Assert.Contains(result.Times, t => t.Unit == "h" && t.Amount == 1);
            Assert.Contains(result.Times, t => t.Unit == "m" && t.Amount == 1);
        }
    }
}
