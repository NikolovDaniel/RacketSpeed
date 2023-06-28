using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for Event Entity.
    /// </summary>
    public class EventService : IEventService
    {
        /// <summary>
        /// Repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        public EventService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task AddAsync(EventFormModel model)
        {
            if (model.ImageUrls == null)
            {
                return;
            }

            var eventEntity = new Event()
            {
                Title = model.Title,
                Content = model.Content,
                Start = model.Start,
                End = model.End,
                Category = model.Category,
                Location = model.Location,
                EventImageUrls = model.ImageUrls
                .Select(img => new EventImageUrl()
                {
                    Url = img
                })
                .ToList(),
                IsDeleted = false
            };

            await this.repository.AddAsync<Event>(eventEntity);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<EventViewModel>> AllAsync(string category, bool isAdministartor)
        {
            Expression<Func<Event, bool>> expression
                = e => e.Category == category && e.IsDeleted == false;

            IQueryable<Event> events = null;

            if (isAdministartor)
            {
                events = this.repository.All<Event>(expression);
            }
            else
            {
                int recentPostsNum = 3;

                events = this.repository.All<Event>(expression).Take(recentPostsNum);
            }


            return await events
                .OrderBy(e => e.Start)
                .Select(e => new EventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Start = e.Start,
                    End = e.End,
                    Location = e.Location,
                    Category = e.Category,
                    TitleImageUrl = e.EventImageUrls.FirstOrDefault().Url 
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid eventId)
        {
            var eventEntity = await this.repository.GetByIdAsync<Event>(eventId);

            if (eventEntity == null)
            {
                return;
            }

            eventEntity.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }

        public async Task EditAsync(EventFormModel model)
        {
            if (model.ImageUrls == null)
            {
                return;
            }

            var eventEntity = await this.repository.GetByIdAsync<Event>(model.Id);

            if (eventEntity == null)
            {
                return;
            }

            eventEntity.Title = model.Title;
            eventEntity.Content = model.Content;
            eventEntity.Start = model.Start;
            eventEntity.End = model.End;
            eventEntity.Location = model.Location;
            eventEntity.Category = model.Category;
            var images = model.ImageUrls.Select(img => new EventImageUrl() { Url = img }).ToList();

            int counter = 0;

            foreach (var item in eventEntity.EventImageUrls)
            {
                if (item.Url != images[counter].Url)
                {
                    item.Url = images[counter].Url;
                }
                counter++;
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task<EventDetailsViewModel> GetByIdAsync(Guid eventId)
        {
            var eventEntity = await this.repository.GetByIdAsync<Event>(eventId);

            if (eventEntity == null)
            {
                return null;
            }

            return new EventDetailsViewModel()
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Content = eventEntity.Content,
                Start = eventEntity.Start,
                End = eventEntity.End,
                Location = eventEntity.Location,
                Category = eventEntity.Category,
                ImageUrls = eventEntity.EventImageUrls
                .Select(img => img.Url)
                .ToArray()
            };
        }

        public async Task<IEnumerable<EventHomePageViewModel>> MostRecentEventsAsync()
        {
            int numberOfPosts = 3;

            var mostRecentPosts = await this.repository
                .AllReadonly<Event>()
                .OrderBy(e => e.Start)
                .Take(numberOfPosts)
                .Select(e => new EventHomePageViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    ImageUrl = e.EventImageUrls.FirstOrDefault().Url
                })
                .ToListAsync();

            return mostRecentPosts;
        }
    }
}

