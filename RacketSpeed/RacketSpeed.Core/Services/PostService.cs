﻿using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for Post Entity.
    /// </summary>
    public class PostService : IPostService
    {
        /// <summary>
        /// Repository.
        /// </summary>
        protected IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        /// <param name="repository"></param>
		public PostService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(PostFormModel model)
        {
            if (model.ImageUrls == null)
            {
                return;
            }

            var post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                IsDeleted = false,
                PostImageUrls = model.ImageUrls
                .Select(img => new PostImageUrl()
                {
                    Url = img
                })
                .ToList(),
            };
          
            await this.repository.AddAsync<Post>(post);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<PostViewModel>> AllAsync()
        {
            var allPosts = this.repository.AllReadonly<Post>();

            return await allPosts
                .Where(p => p.IsDeleted == false)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrls = p.PostImageUrls.Select(img => img.Url).ToArray()
                })
                .ToListAsync();
        }

        public async Task<PostViewModel> GetByIdAsync(Guid id)
        {
            var post = await this.repository.GetByIdAsync<Post>(id);

            if (post == null)
            {
                return null;
            }

            var model = new PostViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrls = post.PostImageUrls.Select(img => img.Url).ToArray()
            };

            return model;
        }

        public async Task EditAsync(PostFormModel model)
        {
            if (model.ImageUrls == null)
            {
                return;
            }

            var post = await this.repository.GetByIdAsync<Post>(model.Id);

            if (post == null)
            {
                return;
            }

            post.Title = model.Title;
            post.Content = model.Content;
            var images = model.ImageUrls.Select(img => new PostImageUrl() { Url = img }).ToList();

            int counter = 0;

            foreach (var item in post.PostImageUrls)
            {
                if (item.Url != images[counter].Url)
                {
                    item.Url = images[counter].Url;
                }
                counter++;
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await this.repository.GetByIdAsync<Post>(id);

            if (post == null)
            {
                return;
            }

            post.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }
    }
}

