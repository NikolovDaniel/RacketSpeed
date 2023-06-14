using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;
using RacketSpeed.Core.Models.Post;
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
                    Biography = c.Biography
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var coach = await this.repository.GetByIdAsync<Coach>(id);

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

            coach.FirstName = model.FirstName;
            coach.LastName = model.LastName;
            coach.Biography = model.Biography;

            await this.repository.SaveChangesAsync();
        }

        public async Task<CoachTrainingsViewModel> GetByIdAsync(Guid id, bool withTrainings)
        {
            var coach = await this.repository.GetByIdAsync<Coach>(id);

            if (coach == null)
            {
                return null;
            }

            var model = new CoachTrainingsViewModel()
            {
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Biography = coach.Biography,
                Trainings = withTrainings ? coach.Trainings
                .Where(t => t.CoachId == coach.Id)
                .Select(t => new TrainingViewModel()
                {
                    Name = t.Name,
                    Start = t.Start,
                    End = t.End
                })
                .ToList() : new List<TrainingViewModel>()
            };

            return model;
        }
    }
}

