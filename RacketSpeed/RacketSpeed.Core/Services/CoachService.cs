using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;
using RacketSpeed.Core.Models.Training;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for Coach Entity.
    /// </summary>
    public class CoachService : ICoachService
    {
        /// <summary>
        /// Repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        public CoachService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task AddAsync(CoachFormModel model)
        {
            var coach = new Coach()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                CoachImageUrl = new CoachImageUrl()
                {
                    Url = model.ImageUrl
                },
                IsDeleted = false
            };

            await this.repository.AddAsync<Coach>(coach);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<CoachViewModel>> AllAsync()
        {
            var allCoaches = this.repository.AllReadonly<Coach>();

            return await allCoaches
                .Where(c => c.IsDeleted == false)
                .Select(c => new CoachViewModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ImageUrl = c.CoachImageUrl.Url
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid coachId)
        {
            var coach = await this.repository.GetByIdAsync<Coach>(coachId);

            if (coach == null)
            {
                return;
            }

            coach.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }

        public async Task EditAsync(CoachFormModel model)
        {
            var coach = await this.repository.GetByIdAsync<Coach>(model.Id);

            if (coach == null)
            {
                return;
            }

            coach.CoachImageUrl.Url = model.ImageUrl;
            coach.FirstName = model.FirstName;
            coach.LastName = model.LastName;
            coach.Biography = model.Biography;

            await this.repository.SaveChangesAsync();
        }

        public async Task<CoachFormModel> GetByIdAsync(Guid coachId)
        {
            var coach = await this.repository.GetByIdAsync<Coach>(coachId);

            return new CoachFormModel()
            {
                Id = coach.Id,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Biography = coach.Biography,
                ImageUrl = coach.CoachImageUrl.Url
            };
        }

        public async Task<CoachTrainingsViewModel> GetByIdAsync(Guid coachId, bool withTrainings)
        {
            var coach = await this.repository.GetByIdAsync<Coach>(coachId);

            Expression<Func<Training, bool>> expression
                = ct => ct.CoachId == coachId && ct.IsDeleted == false;

            var coachTrainings = this.repository.All(expression);

            if (coach == null)
            {
                return null;
            }

            var model = new CoachTrainingsViewModel()
            {
                ImageUrl = coach.CoachImageUrl.Url,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Biography = coach.Biography,
                Trainings = withTrainings && coachTrainings != null ? coachTrainings
                .Select(t => new TrainingViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Start = t.Start,
                    End = t.End,
                    DayOfWeek = t.DayOfWeek
                })
                .ToList() : new List<TrainingViewModel>()
            };

            return model;
        }
    }
}

