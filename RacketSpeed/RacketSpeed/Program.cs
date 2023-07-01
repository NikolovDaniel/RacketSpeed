using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICoachService, CoachService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<ITrainingService, TrainingService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<ISignKidService, SignKidService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            var app = builder.Build();

            //using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //    // Apply any pending migrations
            //    dbContext.Database.Migrate();

            //    // Seed the availability data
            //    SeedAvailabilityData(dbContext);
            //}


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        //private static void SeedAvailabilityData(ApplicationDbContext dbContext)
        //{
        //    // Retrieve the Court IDs
        //    Guid court1Id = dbContext.Courts.First(x => x.Number == 1).Id;
        //    Guid court2Id = dbContext.Courts.First(x => x.Number == 2).Id;
        //    Guid court3Id = dbContext.Courts.First(x => x.Number == 3).Id;
        //    Guid court4Id = dbContext.Courts.First(x => x.Number == 4).Id;

        //    // Define the start and end times for the availability slots
        //    TimeSpan startTime = TimeSpan.FromHours(9); // Start time: 09:00 AM
        //    TimeSpan endTime = TimeSpan.FromHours(21); // End time: 21:00 PM
        //    TimeSpan trainingDuration = TimeSpan.FromHours(1); // Training duration: 1:00 Hour

        //    // Define the duration of each availability slot
        //    TimeSpan timeSlotDuration = TimeSpan.FromHours(1); // 1-hour time slots

        //    while (startTime <= endTime)
        //    {
        //        dbContext.Schedule.Add(new Schedule() { Hour = startTime });

        //        startTime = startTime.Add(timeSlotDuration);
        //    }

        //    // Save the changes to the database
        //    dbContext.SaveChanges();
        //}
    }
}