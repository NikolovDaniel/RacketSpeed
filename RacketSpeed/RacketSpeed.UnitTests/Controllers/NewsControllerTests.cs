using System;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Controllers
{
    public class NewsControllerTests
    {
        private NewsController _controller;
        private Mock<IPostService> _postServiceMock;
        private ClaimsPrincipal Administrator = new ClaimsPrincipal(new ClaimsIdentity(new[]
           {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "Administrator")
            }, "mock"));
        private ClaimsPrincipal RegularUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
          {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "RegularUser")
            }, "mock"));

        [SetUp]
        public void Setup()
        {
            _postServiceMock = new Mock<IPostService>();
            _controller = new NewsController(_postServiceMock.Object);
        }

        [Test]
        public async Task All_GET_ReturnsViewResult()
        {
            // Arrange
            var pageCount = "1";
            var pageNum = 1;
            var postsPerPage = 3;
            var pagesCount = 2;
            var posts = new List<PostViewModel>
            {
                new PostViewModel()
                {
                    Id = Guid.NewGuid(),
                    Title = SeedPropertyConstants.PostTitle1,
                    Content = SeedPropertyConstants.PostContent1,
                    ImageUrls = new string[3] {"url 1", "url 2", "url 3"}
                }
            };
            _postServiceMock.Setup(m => m.PostsPageCount(postsPerPage)).Returns(pagesCount);
            _postServiceMock.Setup(m => m.AllAsync(pageNum, postsPerPage)).ReturnsAsync(posts);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = Administrator };

            // Act
            var result = await _controller.All(pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<PostsPaginationCountViewModel>();
            model.Posts.Should().BeEquivalentTo(posts);
            model.PageCount.Should().Be(pagesCount);
            viewResult.ViewData.Should().ContainKey("IsAdministrator");
            viewResult.ViewData["IsAdministrator"].Should().Be(true);
            viewResult.ViewData.Should().ContainKey("pageNum");
            viewResult.ViewData["pageNum"].Should().Be(pageNum);
        }

        [Test]
        public async Task AllPostsByKeyword_GET_WithInvalidKeyword_ReturnsViewResult()
        {
            // Arrange
            var pageCount = "1";
            var keyword = "";
            var pageNum = 1;
            var postsPerPage = 5;
            var pagesCount = 3;
            var posts = new List<PostViewModel>
            {
                new PostViewModel()
                {
                  Id = Guid.NewGuid(),
                  Title = SeedPropertyConstants.PostTitle1,
                  Content = SeedPropertyConstants.PostContent1,
                  ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
                }
              };
            _postServiceMock.Setup(m => m.PostsPageCount(postsPerPage, keyword)).Returns(pagesCount);
            _postServiceMock.Setup(m => m.AllAsync(pageNum, postsPerPage, keyword)).ReturnsAsync(posts);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = Administrator };

            // Act
            var result = await _controller.AllPostsByKeyword(keyword, pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeNull();
            _controller.ModelState.Should().ContainKey("KeywordError");
        }

        [Test]
        public async Task AllPostsByKeyword_GET_ReturnsViewResult()
        {
            // Arrange
            var pageCount = "1";
            var keyword = "test";
            var pageNum = 1;
            var postsPerPage = 5;
            var pagesCount = 3;
            var posts = new List<PostViewModel>
            {
                new PostViewModel()
                {
                  Id = Guid.NewGuid(),
                  Title = SeedPropertyConstants.PostTitle1,
                  Content = SeedPropertyConstants.PostContent1,
                  ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
                }
              };
            _postServiceMock.Setup(m => m.PostsPageCount(postsPerPage, keyword)).Returns(pagesCount);
            _postServiceMock.Setup(m => m.AllAsync(pageNum, postsPerPage, keyword)).ReturnsAsync(posts);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = Administrator };

            // Act
            var result = await _controller.AllPostsByKeyword(keyword, pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<PostsPaginationCountViewModel>();
            model.Posts.Should().BeEquivalentTo(posts);
            model.PageCount.Should().Be(pagesCount);
            viewResult.ViewData.Should().ContainKey("IsAdministrator");
            viewResult.ViewData["IsAdministrator"].Should().Be(true);
            viewResult.ViewData.Should().ContainKey("keyword");
            viewResult.ViewData["keyword"].Should().Be(keyword);
            viewResult.ViewData.Should().ContainKey("pageNum");
            viewResult.ViewData["pageNum"].Should().Be(pageNum);
        }

        [Test]
        public void Add_GET_ReturnsViewResult()
        {
            // Act
            var result = _controller.Add();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<PostFormModel>();
        }

        [Test]
        public async Task Add_POST_WithValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var model = new PostFormModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _postServiceMock.Setup(m => m.AddAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("News");
        }

        [Test]
        public async Task Add_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            PostFormModel model = null;
            _controller.ModelState.AddModelError("Invalid data", "Invalid Data");

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
            _controller.ModelState.Should().ContainKey("Invalid data");
        }

        [Test]
        public async Task Add_POST_WithNullImages_ReturnsViewResult()
        {
            // Arrange
            var model = new PostFormModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", null }
            };
            _controller.ModelState.AddModelError("UrlEmpty", "All image URLs must be provided");

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
            _controller.ModelState.Should().ContainKey("UrlEmpty");
        }

        [Test]
        public async Task Edit_GET_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new PostViewModel
            {
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _postServiceMock.Setup(m => m.GetByIdAsync(postId)).ReturnsAsync(post);

            // Act
            var result = await _controller.Edit(postId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<PostFormModel>();
            model.Id.Should().Be(post.Id);
            model.Title.Should().Be(post.Title);
            model.Content.Should().Be(post.Content);
            model.ImageUrls.Should().BeEquivalentTo(post.ImageUrls);
        }

        [Test]
        public async Task Edit_GET_WithInvalidId_ReturnsRedirectToAction()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _postServiceMock.Setup(m => m.GetByIdAsync(postId)).ReturnsAsync((PostViewModel)null);

            // Act
            var result = await _controller.Edit(postId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("News");
        }

        [Test]
        public async Task Edit_POST_WithValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var model = new PostFormModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _postServiceMock.Setup(m => m.EditAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("News");
        }

        [Test]
        public async Task Edit_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new PostFormModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _controller.ModelState.AddModelError("InvalidDates", "Start date must be earlier than end date");

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Delete_POST_ReturnsRedirectToAction()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _postServiceMock.Setup(m => m.DeleteAsync(postId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(postId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("News");
        }

        [Test]
        public async Task Details_GET_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new PostViewModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _postServiceMock.Setup(m => m.GetByIdAsync(postId)).ReturnsAsync(post);

            // Act
            var result = await _controller.Details(postId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(post);
        }

        [Test]
        public async Task Details_GET_WithInvalidId_ReturnsRedirectToAction()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _postServiceMock.Setup(m => m.GetByIdAsync(postId)).ReturnsAsync((PostViewModel)null);

            // Act
            var result = await _controller.Details(postId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("News");
        }
    }
}

