using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;
using RacketSpeed.Core.Models.Training;

namespace RacketSpeed.UnitTests.Controllers
{
    public class TrainingControllerTests
    {
        private TrainingController _trainingController;
        private Mock<ICoachService> _coachServiceMock;
        private Mock<ITrainingService> _trainingServiceMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _coachServiceMock = new Mock<ICoachService>();
            _trainingServiceMock = new Mock<ITrainingService>();
            _trainingController = new TrainingController(_coachServiceMock.Object, _trainingServiceMock.Object);
            _fixture = new Fixture();
        }

        [Test]
        public async Task All_WithTrainingName_ReturnsViewResultWithAllTrainings()
        {
            // Arrange
            string trainingName = "Деца до 19";
            var allTrainings = _fixture.CreateMany<TrainingViewModel>().ToList();
            _trainingServiceMock.Setup(s => s.AllAsync(trainingName)).ReturnsAsync(allTrainings);

            // Act
            var result = await _trainingController.All(trainingName);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeEquivalentTo(allTrainings);
            viewResult.ViewData["Title"].Should().Be($"{trainingName} години");
            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task Add_GET_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);

            // Act
            var result = await _trainingController.Add();

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var model = viewResult.Model.As<TrainingFormModel>();
            model.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task Add_POST_WithInvalidStartTime_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>()
                .With(t => t.Start, DateTime.Now.AddHours(1))
                .With(t => t.End, DateTime.Now)
                .Create();
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);
            _coachServiceMock
                .Setup(s => s.HasTraining(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            var result = await _trainingController.Add(model);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
            _trainingController.ModelState.Should().ContainKey("InvalidHours");
        }

        [Test]
        public async Task Add_POST_WithNoAvailableTimeForTraining_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>().Create();
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);
            _coachServiceMock
                .Setup(s => s.HasTraining(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(true);

            // Act
            var result = await _trainingController.Add(model);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
            _trainingController.ModelState.Should().ContainKey("InvalidTrainingTime");
        }

        [Test]
        public async Task Add_POST_WithInvalidModelState_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>().Create();
            _trainingController.ModelState.AddModelError("InvalidData", "Invalid data.");

            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);
            _coachServiceMock
                .Setup(s => s.HasTraining(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            var result = await _trainingController.Add(model);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
            _trainingController.ModelState.Should().ContainKey("InvalidData");
        }

        [Test]
        public async Task Add_POST_WithValidModelState_CallsTrainingServiceAddAsyncAndRedirectsToAllActionInCoachController()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>()
                .With(t => t.Start, DateTime.Now)
                .With(t => t.End, DateTime.Now.AddHours(2))
                .Create();
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);

            // Act
            var result = await _trainingController.Add(model);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<RedirectToActionResult>();
            viewResult.ActionName.Should().Be("All");
            viewResult.ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Edit_GET_WithExistingTrainingId_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var trainingId = Guid.NewGuid();
            var training = _fixture.Build<TrainingFormModel>().With(t => t.Id, trainingId).Create();
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _trainingServiceMock.Setup(s => s.GetByIdAsync(trainingId)).ReturnsAsync(training);
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);

            // Act
            var result = await _trainingController.Edit(trainingId);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Should().BeEquivalentTo(training);
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task Edit_GET_WithNonExistingTrainingId_RedirectsToAllActionInCoachController()
        {
            // Arrange
            var trainingId = Guid.NewGuid();
            _trainingServiceMock.Setup(s => s.GetByIdAsync(trainingId)).ReturnsAsync((TrainingFormModel)null);

            // Act
            var result = await _trainingController.Edit(trainingId);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<RedirectToActionResult>();
            viewResult.ActionName.Should().Be("All");
            viewResult.ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Edit_POST_WithNoAvailableTimeForTraining_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>().Create();
            _trainingController.ModelState.AddModelError("InvalidTrainingTime", "InvalidTrainingTime.");
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);
            _coachServiceMock
                          .Setup(s => s.HasTraining(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                          .Returns(true);

            // Act
            var result = await _trainingController.Edit(model) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
            _trainingController.ModelState.Should().ContainKey("InvalidTrainingTime");
        }

        [Test]
        public async Task Edit_POST_WithInvalidStartTime_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>()
                .With(t => t.Start, DateTime.Now.AddHours(2))
                .With(t => t.End, DateTime.Now)
                .Create();
            _trainingController.ModelState.AddModelError("InvalidHours", "Invalid hours.");
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);
            _coachServiceMock
                          .Setup(s => s.HasTraining(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                          .Returns(false);

            // Act
            var result = await _trainingController.Edit(model) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
            _trainingController.ModelState.Should().ContainKey("InvalidHours");
        }

        [Test]
        public async Task Edit_POST_WithInvalidModelState_ReturnsViewResultWithTrainingFormModelAndCoaches()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>().Create();
            _trainingController.ModelState.AddModelError("InvalidData", "Invalid data.");
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);
            _coachServiceMock
                          .Setup(s => s.HasTraining(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                          .Returns(false);

            // Act
            var result = await _trainingController.Edit(model) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<TrainingFormModel>();
            var resultModel = viewResult.Model.As<TrainingFormModel>();
            resultModel.Coaches.Count().Should().Be(coaches.Count());
            viewResult.ViewName.Should().BeNull();
            _trainingController.ModelState.Should().ContainKey("InvalidData");
        }

        [Test]
        public async Task Edit_POST_WithValidModelState_CallsTrainingServiceEditAsyncAndRedirectsToAllActionInCoachController()
        {
            // Arrange
            var model = _fixture.Build<TrainingFormModel>()
            .With(t => t.Start, DateTime.Now)
            .With(t => t.End, DateTime.Now.AddHours(2))
            .Create();
            var coaches = _fixture.CreateMany<CoachViewModel>().ToList();
            _coachServiceMock.Setup(s => s.AllAsync()).ReturnsAsync(coaches);


            // Act
            var result = await _trainingController.Edit(model);

            // Assert
            result.Should().NotBeNull();
            var viewResult = result.As<RedirectToActionResult>();
            viewResult.ActionName.Should().Be("All");
            viewResult.ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Delete_POST_CallsTrainingServiceDeleteAsyncAndRedirectsToAllActionInCoachController()
        {
            // Arrange
            var trainingId = Guid.NewGuid();

            // Act
            var result = await _trainingController.Delete(trainingId) as RedirectToActionResult;

            // Assert
            result.Should().NotBeNull();
            result.ActionName.Should().Be("All");
            result.ControllerName.Should().Be("Coach");
            _trainingServiceMock.Verify(s => s.DeleteAsync(trainingId), Times.Once);
        }
    }
}

