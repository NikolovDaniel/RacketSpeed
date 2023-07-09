using RacketSpeed.Core.Models.Coach;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.TestsData
{
    public class CoachServiceTestData
    {
        public static CoachFormModel ValidCoachForm()
        {
            return new CoachFormModel()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Biography = "Lorem ipsum, Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,",
                ImageUrl = "Lorem ipsum,Lorem ipsum,"
            };
        }

        public static List<Training> ListWithTrainings()
        {
            return new List<Training>()
            {
                 new Training()
                {
                    Id = Guid.NewGuid(),
                    Name = SeedPropertyConstants.Coach1Training1Name,
                    Start = DateTime.Parse("2023-06-27 20:00:00.000"),
                    End = DateTime.Parse("2023-06-27 21:00:00.000"),
                    DayOfWeek = SeedPropertyConstants.Coach1Training1DayOfWeek,
                    CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                    IsDeleted = false
                },
               new Training()
               {
                   Id = Guid.NewGuid(),
                   Name = SeedPropertyConstants.Coach1Training2Name,
                   Start = DateTime.Parse("2023-06-27 19:00:00.000"),
                   End = DateTime.Parse("2023-06-27 20:00:00.000"),
                   DayOfWeek = SeedPropertyConstants.Coach1Training2DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                   IsDeleted = false
               }
            };
        }

        public static List<Coach> ListWithCoaches()
        {
            return new List<Coach>()
            {
                new Coach()
                {
                    Id = Guid.Parse("65DE1416-56AB-493F-A24B-8A6E23F77426"),
                    FirstName = SeedPropertyConstants.CoachFirstName1,
                    LastName = SeedPropertyConstants.CoachLastName1,
                    Biography = SeedPropertyConstants.CoachBiography1,
                    IsDeleted = false
                },
                new Coach()
                {
                    Id = Guid.Parse("EFF8FA04-F157-40F7-8866-9DA4E3EBB28E"),
                    FirstName = SeedPropertyConstants.CoachFirstName2,
                    LastName = SeedPropertyConstants.CoachLastName2,
                    Biography = SeedPropertyConstants.CoachBiography2,
                    IsDeleted = false
                },
                new Coach()
                {
                    Id = Guid.Parse("664F5EAF-D3CB-45A2-9245-6BF4B693CC15"),
                    FirstName = SeedPropertyConstants.CoachFirstName3,
                    LastName = SeedPropertyConstants.CoachLastName3,
                    Biography = SeedPropertyConstants.CoachBiography3,
                    IsDeleted = false
                }
            };
        }
    }
}

