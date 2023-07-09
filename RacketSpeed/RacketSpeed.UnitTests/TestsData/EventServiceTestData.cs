using RacketSpeed.Core.Models.Event;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.TestsData
{
    public class EventServiceTestData
    {

        public static EventFormModel InvalidEventFormModel()
        {
            return new EventFormModel()
            {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = null
            };
        }

        public static List<EventFormModel> ListWithEventFormModels()
        {
            return new List<EventFormModel>()
            {
                new EventFormModel()
                {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] {"url 1", "url 2", "url 3"}
                },
                new EventFormModel()
                {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] {"url 1", "url 2", "url 3"}
                },
                new EventFormModel()
                {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] {"url 1", "url 2", "url 3"}
                }
            };
        }
    }
}

