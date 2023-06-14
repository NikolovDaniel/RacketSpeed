using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Player;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for Player Entity.
    /// </summary>
    public class PlayerService : IPlayerService
	{
        /// <summary>
        /// Repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        public PlayerService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task AddAsync(PlayerFormModel model)
        {
            var post = new Player()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                Ranking = model.Ranking
            };

            await this.repository.AddAsync<Player>(post);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<PlayerViewModel>> AllAsync()
        {
            var allPlayers = this.repository.AllReadonly<Player>();

            return await allPlayers
                .Where(p => p.IsDeleted == false)
                .Select(p => new PlayerViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Biography = p.Biography,
                    BirthDate = p.BirthDate,
                    Ranking = p.Ranking
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var player = await this.repository.GetByIdAsync<Player>(id);

            if (player == null)
            {
                return;
            }

            player.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }

        public async Task EditAsync(PlayerFormModel model)
        {
            var player = await this.repository.GetByIdAsync<Player>(model);

            if (player == null)
            {
                return;
            }

            player.FirstName = model.FirstName;
            player.LastName = model.LastName;
            player.BirthDate = model.BirthDate;
            player.Ranking = model.Ranking;
            player.Biography = model.Biography;

            await this.repository.SaveChangesAsync();
        }

        public async Task<PlayerViewModel> GetByIdAsync(Guid id)
        {
            var player = await this.repository.GetByIdAsync<Player>(id);

            if (player == null)
            {
                return null;
            }

            var model = new PlayerViewModel()
            {
                FirstName = player.FirstName,
                LastName = player.LastName,
                Biography = player.Biography,
                BirthDate = player.BirthDate,
                Ranking = player.Ranking
            };

            return model;
        }
    }
}

