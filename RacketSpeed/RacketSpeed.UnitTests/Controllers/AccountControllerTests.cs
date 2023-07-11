using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Controllers;
using RacketSpeed.Email;
using RacketSpeed.Infrastructure.Data.Entities;

namespace RacketSpeed.UnitTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<SendGridEmailSender> _sendGridEmailSenderMock;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _sendGridEmailSenderMock = new Mock<SendGridEmailSender>();
            _controller = new AccountController(_userManagerMock.Object, _sendGridEmailSenderMock.Object);
        }

        [Test]
        public void ResendConfirmationEmail_GET_ReturnsViewResult()
        {
            // Act
            var result = _controller.ResendConfirmationEmail();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        //[Test]
        //public async Task ResendConfirmationEmail_POST_WithValidEmail_ReturnsRedirectToActionResult()
        //{
        //    // Arrange
        //    var user = new ApplicationUser {Id = Guid.NewGuid().ToString(), Email = "test@example.com", EmailConfirmed = false };
        //    _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        //    _userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("token");

        //    // Act
        //    var result = await _controller.ResendConfirmationEmail("test@example.com");

        //    // Assert
        //    result.Should().BeOfType<RedirectToActionResult>();
        //    var redirectResult = result.As<RedirectToActionResult>();
        //    redirectResult.ActionName.Should().Be("Index");
        //    redirectResult.ControllerName.Should().Be("Home");
        //}

        [Test]
        public async Task ResendConfirmationEmail_POST_WithInvalidEmail_ReturnsViewResultWithModelError()
        {
            // Act
            var result = await _controller.ResendConfirmationEmail(null);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewData.ModelState.ErrorCount.Should().Be(1);
            viewResult.ViewData.ModelState["InvalidEmail"].Errors.Should().Contain(e => e.ErrorMessage == "Полето е задължително.");
        }
    }
}

