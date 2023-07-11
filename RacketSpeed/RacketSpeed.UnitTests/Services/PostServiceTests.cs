using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;
using RacketSpeed.UnitTests.TestsData;

namespace RacketSpeed.UnitTests.Services
{
    public class PostServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
           = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("racketspeedtests")
          .Options;

        private Mock<Repository> repository;
        private IPostService postService;
        private ApplicationDbContext context;

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            postService = new PostService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public async Task PostsPageCount_WithoutKeyword_ReturnsCorrectCount(int postsPerPage)
        {
            // Arrange
            int allPostsCount = await this.context.Posts
                .Where(p => p.IsDeleted == false)
                .CountAsync();
            int pageCount = (int)Math.Ceiling((allPostsCount / (double)postsPerPage));
            int expectedPageCount = pageCount == 0 ? 1 : pageCount;

            // Act
            int actualPageCount = this.postService.PostsPageCount(postsPerPage);

            // Assert
            actualPageCount.Should().Be(expectedPageCount);
        }

        [Test]
        [TestCase(1, "баД")]
        [TestCase(2, "БадминТон клУБ")]
        [TestCase(3, "Днес се сбъдна една голяма мечта")]
        public async Task PostsPageCount_WithKeyword_ReturnsCorrectCount(int postsPerPage, string keyword)
        {
            // Arrange
            int allPostsCount = await this.context.Posts
                .Where(p => p.IsDeleted == false && p.Title.ToUpper().Contains(keyword.ToUpper()))
                .CountAsync();
            int pageCount = (int)Math.Ceiling((allPostsCount / (double)postsPerPage));
            int expectedPageCount = pageCount == 0 ? 1 : pageCount;

            // Act
            int actualPageCount = this.postService.PostsPageCount(postsPerPage, keyword);

            // Assert
            actualPageCount.Should().Be(expectedPageCount);
        }

        [Test]
        [TestCase(SeedPropertyConstants.PostId1)]
        [TestCase(SeedPropertyConstants.PostId2)]
        [TestCase(SeedPropertyConstants.PostId3)]
        public async Task DeleteAsync__WithValidId_DeletesCorrectly(Guid postId)
        {
            // Arrange
            var postEntity = await this.context.Posts.FindAsync(postId);

            // Act
            await this.postService.DeleteAsync(postId);

            // Assert
            postEntity.Should().Match<Post>(c => c.IsDeleted == true, "we deleted the post");
        }

        [Test]
        [TestCase(SeedPropertyConstants.PostId1)]
        [TestCase(SeedPropertyConstants.PostId2)]
        [TestCase(SeedPropertyConstants.PostId3)]
        public async Task EditAsync_WithValidId_EditsCorrectly(Guid postId)
        {
            // Arrange
            var model = new PostFormModel()
            {
                Id = postId,
                Title = SeedPropertyConstants.PostTitle3,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };

            // Act
            await this.postService.EditAsync(model);

            // Assert
            var modifiedEntity = await this.context.Posts
                .Include(e => e.PostImageUrls)
                .FirstOrDefaultAsync(e => e.Id == postId);

            modifiedEntity.Should().NotBeNull();
            modifiedEntity.Should().NotBeEquivalentTo(model, "we changed the entity properties");
            modifiedEntity.Id.Should().Be(model.Id);
            modifiedEntity.Title.Should().Be(model.Title);
            modifiedEntity.Content.Should().Be(model.Content);
            modifiedEntity.Title.Should().Be(model.Title);
            modifiedEntity.PostImageUrls.Should().OnlyContain(e => e.Url.Contains("url"));
        }

        [Test]
        [TestCase(SeedPropertyConstants.PostId1)]
        [TestCase(SeedPropertyConstants.PostId2)]
        [TestCase(SeedPropertyConstants.PostId3)]
        public async Task EditAsync_WithInvalidImages_DoesNotEdit(Guid postId)
        {
            // Arrange
            var originalEntity = await this.context.Posts
               .Include(p => p.PostImageUrls)
               .FirstOrDefaultAsync(p => p.Id == postId);
            var model = new PostFormModel()
            {
                Id = postId,
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
            };

            // Act
            await this.postService.EditAsync(model);

            // Assert
            var modifiedEntity = await this.context.Posts
                .Include(e => e.PostImageUrls)
                .FirstOrDefaultAsync(e => e.Id == postId);

            modifiedEntity.Should().NotBeNull();
            modifiedEntity.Should().BeEquivalentTo(originalEntity, "we passed invalid image urls");
        }

        [Test]
        [TestCase(SeedPropertyConstants.PostId1)]
        [TestCase(SeedPropertyConstants.PostId2)]
        [TestCase(SeedPropertyConstants.PostId3)]
        public async Task GetByIdAsync_WithValidId_ReturnsCorrectPost(Guid postId)
        {
            // Arrange
            var post = await this.context.Posts.FindAsync(postId);
            var expectedPostEntity = new PostViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrls = post.PostImageUrls
                .Select(img => img.Url)
                .ToArray()
            };

            // Act
            var actualPostEntity = await this.postService.GetByIdAsync(postId);

            // Assert
            actualPostEntity.Should().NotBeNull();
            actualPostEntity.Should().BeEquivalentTo(expectedPostEntity, "it is the same post");
        }

        [Test]
        [TestCase(0, 1, "И")]
        [TestCase(0, 2, "баД")]
        [TestCase(0, 3, "БадминТон клУБ")]
        [TestCase(1, 1, "отбОр")]
        [TestCase(2, 1, "Днес се сбъдна една голяма мечта")]
        public async Task AllAsync_WithKeyword_ReturnsCorrectCollection(int start, int postsPerPage, string keyword)
        {
            // Arrange
            int skipCount = (start - 1) * postsPerPage;
            var posts = await this.context.Posts
                .Where(p => p.IsDeleted == false && p.Title.ToUpper().Contains(keyword.ToUpper()))
                .Skip(skipCount)
                .Take(postsPerPage)
                .Include(p => p.PostImageUrls)
                .ToListAsync();
            var expectedPostsCollection = posts
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrls = p.PostImageUrls.Select(img => img.Url).ToArray()
                });
            // Act
            var actualPostsCollection = await this.postService.AllAsync(start, postsPerPage, keyword);

            // Assert
            actualPostsCollection.Should().NotBeNull();
            actualPostsCollection.Should().BeEquivalentTo(expectedPostsCollection, "we have the same collection");
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(0, 3)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        public async Task AllAsync_WithoutKeyword_ReturnsCorrectCollection(int start, int postsPerPage)
        {
            // Arrange
            int skipCount = (start - 1) * postsPerPage;
            var posts = await this.context.Posts
                .Where(p => p.IsDeleted == false)
                .Skip(skipCount)
                .Take(postsPerPage)
                .Include(p => p.PostImageUrls)
                .ToListAsync();
            var expectedPostsCollection = posts
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrls = p.PostImageUrls.Select(img => img.Url).ToArray()
                });
            // Act
            var actualPostsCollection = await this.postService.AllAsync(start, postsPerPage);

            // Assert
            actualPostsCollection.Should().NotBeNull();
            actualPostsCollection.Should().BeEquivalentTo(expectedPostsCollection, "we have the same collection");
        }

        [Test]
        public async Task AddAsync_ModelWithoutImages_DoesNotAdd()
        {
            // Arrange
            var expectedPostsCount = await this.context.Posts.CountAsync();
            var model = PostServiceTestData.InvalidPostFormModel();

            // Act
            await this.postService.AddAsync(model);

            // Assert
            var actualPostsCount = await this.context.Posts.CountAsync();
            actualPostsCount.Should().Be(expectedPostsCount, "we tried to add invalid entity");
        }

        [Test]
        public async Task AddAsync_WithValidModel_AddsCorrectly()
        {
            // Arrange
            var expectedPostsCount = await this.context.Posts.CountAsync();
            var model = PostServiceTestData.InvalidPostFormModel();

            // Act
            await this.postService.AddAsync(model);

            // Assert
            var actualPostsCount = await this.context.Posts.CountAsync();
            actualPostsCount.Should().Be(expectedPostsCount, "we added one entity");
        }
    }
}

