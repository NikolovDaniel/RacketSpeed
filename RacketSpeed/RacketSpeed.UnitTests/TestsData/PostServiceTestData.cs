using RacketSpeed.Core.Models.Post;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.TestsData
{
    public class PostServiceTestData
    {
        public static PostFormModel ValidPostFormModel()
        {
            return new PostFormModel()
            {
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
        }

        public static PostFormModel InvalidPostFormModel()
        {
            return new PostFormModel()
            {
                Title = SeedPropertyConstants.PostTitle1,
                Content = SeedPropertyConstants.PostContent1,
                ImageUrls = { }
            };
        }
    }
}

