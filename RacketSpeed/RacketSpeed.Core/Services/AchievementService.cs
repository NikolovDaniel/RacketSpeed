using System;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Achievement;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for Achievement Entity.
    /// </summary>
    public class AchievementService : IAchievementService
    {
        /// <summary>
        /// Repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        /// <param name="repository"></param>
        public AchievementService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task AddAsync(AchievementFormModel model)
        {
            var achievement = new Achievement()
            {
                Date = model.Date,
                Location = model.Location,
                Content = model.Content
            };

            await this.repository.AddAsync<Achievement>(achievement);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<AchievementViewModel>> AllAsync()
        {
            var allAchievements = this.repository.AllReadonly<Achievement>();

            return await allAchievements
                .Where(a => a.IsDeleted == false)
                .Select(a => new AchievementViewModel()
                {
                    Id = a.Id,
                    Date = a.Date,
                    Location = a.Location,
                    Content = a.Content
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var achievement = await this.repository.GetByIdAsync<Achievement>(id);

            if (achievement == null)
            {
                return;
            }

            achievement.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }

        public async Task EditAsync(AchievementFormModel model)
        {
            var achievement = await this.repository.GetByIdAsync<Achievement>(model.Id);

            if (achievement == null)
            {
                return;
            }

            achievement.Date = model.Date;
            achievement.Location = model.Location;
            achievement.Content = model.Content;

            await this.repository.SaveChangesAsync();
        }

        public async Task<AchievementViewModel> GetByIdAsync(Guid id)
        {
            var achievement = await this.repository.GetByIdAsync<Achievement>(id);

            if (achievement == null)
            {
                return null;
            }

            var model = new AchievementViewModel()
            {
                Id = achievement.Id,
                Date = achievement.Date,
                Location = achievement.Location,
                Content = achievement.Content
            };

            return model;
        }
    }
}

