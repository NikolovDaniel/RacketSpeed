using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Player;
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
                BirthPlace = model.BirthPlace,
                PlayingHand = model.PlayingHand,
                Height = model.Height,
                WorldRanking = model.WorldRanking,
                NationalRanking = model.NationalRanking,
                CreatedOn = model.CreatedOn,
                PlayerImageUrl = new PlayerImageUrl()
                {
                    Url = model.ImageUrl
                }
            };

            await this.repository.AddAsync<Player>(post);
            await this.repository.SaveChangesAsync();
        }

        public int PlayersPageCount(int playersPerPage)
        {
            int allPlayersCount = this.repository
                .AllReadonly<Player>()
                .Where(p => p.IsDeleted == false)
                .Count();

            int pageCount = (int)Math.Ceiling((allPlayersCount / (double)playersPerPage));

            return pageCount;
        }

        public int PlayersPageCount(int playersPerPage, string keyword)
        {
            Expression<Func<Player, bool>> expression
             = p => p.IsDeleted == false &&
                    p.FirstName.ToUpper().Contains(keyword.ToUpper()) ||
                    p.LastName.ToUpper().Contains(keyword.ToUpper());

            int allPlayersCount = this.repository
                .All<Player>(expression)
                .Where(p => p.IsDeleted == false)
                .Count();

            int pageCount = (int)Math.Ceiling((allPlayersCount / (double)playersPerPage));

            return pageCount;
        }

        public async Task<ICollection<PlayerViewModel>> AllAsync(int start, int playersPerPage)
        {
            int currPage = start;
            start = (currPage - 1) * playersPerPage;

            var allPlayers = this.repository
                .AllReadonly<Player>()
                .OrderBy(p => p.CreatedOn)
                .Skip(start)
                .Take(playersPerPage);

            return await allPlayers
                .Where(p => p.IsDeleted == false)
                .Select(p => new PlayerViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    BirthPlace = p.BirthPlace,
                    CreatedOn = p.CreatedOn,
                    Biography = p.Biography,
                    WorldRanking = p.WorldRanking,
                    NationalRanking = p.NationalRanking,
                    PlayingHand = p.PlayingHand,
                    Height = p.Height,
                    ImageUrl = p.PlayerImageUrl.Url
                })
                .ToListAsync();
        }

        public async Task<ICollection<PlayerViewModel>> AllAsync(int start, int playersPerPage, string keyword)
        {
            int currPage = start;
            start = (currPage - 1) * playersPerPage;

            Expression<Func<Player, bool>> expression
             = p => p.IsDeleted == false &&
                    p.FirstName.ToUpper().Contains(keyword.ToUpper()) ||
                    p.LastName.ToUpper().Contains(keyword.ToUpper());

            var allPlayers = this.repository.All<Player>(expression);

            return await allPlayers
                .Select(p => new PlayerViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    BirthPlace = p.BirthPlace,
                    CreatedOn = p.CreatedOn,
                    Biography = p.Biography,
                    WorldRanking = p.WorldRanking,
                    NationalRanking = p.NationalRanking,
                    PlayingHand = p.PlayingHand,
                    Height = p.Height,
                    ImageUrl = p.PlayerImageUrl.Url
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid playerId)
        {
            var player = await this.repository.GetByIdAsync<Player>(playerId);

            if (player == null)
            {
                return;
            }

            player.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }

        public async Task EditAsync(PlayerFormModel model)
        {
            var player = await this.repository.GetByIdAsync<Player>(model.Id);

            if (player == null)
            {
                return;
            }

            player.FirstName = model.FirstName;
            player.LastName = model.LastName;
            player.WorldRanking = model.WorldRanking;
            player.NationalRanking = model.NationalRanking;
            player.Biography = model.Biography;
            player.BirthDate = model.BirthDate;
            player.BirthPlace = model.BirthPlace;
            player.PlayingHand = model.PlayingHand;
            player.Height = model.Height;
            player.PlayerImageUrl.Url = model.ImageUrl;

            await this.repository.SaveChangesAsync();
        }

        public async Task<PlayerFormModel> GetByIdAsync(Guid playerId)
        {
            var player = await this.repository.GetByIdAsync<Player>(playerId);

            if (player == null)
            {
                return null;
            }

            var model = new PlayerFormModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Biography = player.Biography,
                BirthDate = player.BirthDate,
                BirthPlace = player.BirthPlace,
                WorldRanking = player.WorldRanking,
                Height = player.Height,
                PlayingHand = player.PlayingHand,
                NationalRanking = player.NationalRanking,
                CreatedOn = player.CreatedOn,
                ImageUrl = player.PlayerImageUrl.Url
            };

            return model;
        }

        public async Task<int> PlayersCountAsync()
        {
            int playersCount = await this.repository.AllReadonly<Player>().CountAsync();

            return playersCount;
        }
    }
}

