using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Infrastructure.Data.Entities;
using System.Reflection.Emit;

namespace RacketSpeed.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<Coach> Coaches { get; set; } = null!;
        public DbSet<Court> Courts { get; set; } = null!;
        public DbSet<CourtSchedule> CourtSchedules { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;
        public DbSet<PostImageUrl> PostImageUrls { get; set; } = null!;
        public DbSet<PlayerImageUrl> PlayerImageUrls { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Fluent API to configure a Player to always come with its ImageUrls.
            builder.Entity<Player>().Navigation(img => img.PlayerImageUrl).AutoInclude();
            // Fluent API to configure a Post to always come with its ImageUrls.
            builder.Entity<Post>().Navigation(img => img.PostImageUrls).AutoInclude();

            // Fluent API to configure Court and Schedule Entities for Many-To-Many.
            builder.Entity<CourtSchedule>()
               .HasOne(ur => ur.Court)
               .WithMany(u => u.CourtSchedules)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CourtSchedule>()
              .HasOne(ur => ur.Schedule)
              .WithMany(u => u.CourtSchedules)
              .OnDelete(DeleteBehavior.Restrict);

            // Fluent API to configure ApplicationUser and Reservation Entities for One-To-Many.
            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Reservations)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Restrict);

            ////Seeding the relation between our user and role to AspNetUserRoles table
            //builder.Entity<IdentityRole>()
            //    .HasData(new IdentityRole
            //    {
            //        Id = new Guid().ToString(),
            //        Name = "Administrator",
            //        NormalizedName = "ADMINISTRATOR"
            //    });


            base.OnModelCreating(builder);
        }
    }
}