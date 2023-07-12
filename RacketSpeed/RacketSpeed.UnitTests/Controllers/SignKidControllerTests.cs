using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.SignKidForm;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Controllers
{
    [TestFixture]
    public class SignKidControllerTests
    {
        private SignKidController _controller;
        private Mock<ISignKidService> _signKidServiceMock;

        [SetUp]
        public void Setup()
        {
            _signKidServiceMock = new Mock<ISignKidService>();
            _controller = new SignKidController(_signKidServiceMock.Object);
        }

        [Test]
        public void SignForm_GET_ReturnsViewResult()
        {
            // Act
            var result = _controller.SignForm();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<SignKidFormModel>();
        }

        [Test]
        public async Task SignForm_POST_WithValidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new SignKidFormModel
            {
                // Set valid properties
                PrivacyPolicyIsAccepted = true
            };
            _signKidServiceMock.Setup(m => m.AddAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SignForm(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().BeNull();
            viewResult.ViewData.Should().ContainKey("SuccessfullyAdded");
        }

        [Test]
        public async Task SignForm_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new SignKidFormModel
            {
                PrivacyPolicyIsAccepted = false
            };
            _controller.ModelState.AddModelError("InvalidProperty", "Invalid property value");

            // Act
            var result = await _controller.SignForm(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
            _controller.ModelState.Should().ContainKey("InvalidProperty");
        }

        [Test]
        public async Task SignForm_POST_WithPrivacyPolicyNotAccepted_ReturnsViewResult()
        {
            // Arrange
            var model = new SignKidFormModel
            {
                PrivacyPolicyIsAccepted = false
            };

            // Act
            var result = await _controller.SignForm(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
            _controller.ModelState.IsValid.Should().BeFalse();
            _controller.ModelState.Should().ContainKey("PrivacyPolicy");
        }

        [Test]
        public async Task All_GET_ReturnsViewResult()
        {
            // Arrange
            var pageCount = "1";
            var pageNum = 1;
            var formsPerPage = 6;
            var pagesCount = 3;
            var signedKidForms = new List<SignKidFormViewModel>
            {
                new SignKidFormViewModel()
                {
                    PhoneNumber = SeedPropertyConstants.SignFormPhoneNumber1,
                    EmailAddress = SeedPropertyConstants.SignFormEmailAddress1,
                    FullName = SeedPropertyConstants.SignFormFullName2,
                    SignOn = DateTime.Now
                }
            };
            _signKidServiceMock.Setup(m => m.SignFormsPageCount(formsPerPage)).Returns(pagesCount);
            _signKidServiceMock.Setup(m => m.AllAsync(pageNum, formsPerPage)).ReturnsAsync(signedKidForms);

            // Act
            var result = await _controller.All(pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<SignKidFormsPaginationViewModel>();
            model.SignedKidForms.Should().BeEquivalentTo(signedKidForms);
            model.PageCount.Should().Be(pagesCount);
            viewResult.ViewData.Should().ContainKey("pageNum");
            viewResult.ViewData["pageNum"].Should().Be(pageNum);
        }

        [Test]
        public async Task Details_GET_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var signKidFormId = Guid.NewGuid();
            var signKidForm = new SignKidFormModel
            {
                PhoneNumber = SeedPropertyConstants.SignFormPhoneNumber1,
                EmailAddress = SeedPropertyConstants.SignFormEmailAddress1,
                FullName = SeedPropertyConstants.SignFormFullName2,
                SignOn = DateTime.Now,
                TrainingType = SeedPropertyConstants.SignFormTrainingType3,
                Location = SeedPropertyConstants.SignFormLocation2,
                PrivacyPolicyIsAccepted = true,
                Message = "Няма съобщение"

            };
            _signKidServiceMock.Setup(m => m.GetByIdAsync(signKidFormId)).ReturnsAsync(signKidForm);

            // Act
            var result = await _controller.Details(signKidFormId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().Be(signKidForm);
        }

        [Test]
        public async Task Details_GET_WithInvalidId_ReturnsRedirectToAction()
        {
            // Arrange
            var signKidFormId = Guid.NewGuid();
            _signKidServiceMock.Setup(m => m.GetByIdAsync(signKidFormId)).ReturnsAsync((SignKidFormModel)null);

            // Act
            var result = await _controller.Details(signKidFormId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("SignKid");
        }

        [Test]
        public async Task Delete_POST_WithValidId_ReturnsRedirectToAction()
        {
            // Arrange
            var signKidFormId = Guid.NewGuid();
            _signKidServiceMock.Setup(m => m.DeleteAsync(signKidFormId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(signKidFormId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("SignKid");
        }
    }
}

