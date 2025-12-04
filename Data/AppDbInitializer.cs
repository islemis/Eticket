using Microsoft.AspNetCore.Identity;
using Ticket.Data.Static;
using Ticket.Models;

namespace Ticket.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                // 1️⃣ Producers
                if (!context.Producers.Any())
                {
                    var producers = new List<Producer>
            {
                new Producer { Name = "Christopher Nolan" },
                new Producer { Name = "Steven Spielberg" }
            };
                    context.Producers.AddRange(producers);
                    context.SaveChanges();
                }

          
                // 3️⃣ Actors
                if (!context.Actors.Any())
                {
                    var actors = new List<Actor>
            {
                new Actor { Name = "Leonardo DiCaprio" },
                new Actor { Name = "Joseph Gordon-Levitt" },
                new Actor { Name = "Matthew McConaughey" }
            };
                    context.Actors.AddRange(actors);
                    context.SaveChanges();
                }

                // 4️⃣ Movies
                // 4️⃣ Movies
                if (!context.Movies.Any())
                {
                    var firstProducerId = context.Producers.First().Id;

                    var movies = new List<Movie>
    {
        new Movie
        {
            Title = "Inception",
            Description = "Un film de science-fiction sur les rêves partagés.",
            DurationMinutes = 148,
            ImageURL = "https://localhost:7021/images/inception.jpg",   
            ProducerId = firstProducerId,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(1)
        },
        new Movie
        {
            Title = "Interstellar",
            Description = "Voyage dans l'espace pour sauver l'humanité.",
            DurationMinutes = 169,
            ImageURL = "/images/interstellar.jpg", 
            ProducerId = firstProducerId,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(1)
        },
        new Movie
        {
            Title = "Dune: Part One",
            Description = "Science-fiction épique.",
            DurationMinutes = 155,
            ImageURL = "/images/dune.jpg",  
            ProducerId = firstProducerId,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(2)
        }
    };

                    context.Movies.AddRange(movies);
                    context.SaveChanges();
                }

                // 5️⃣ Relations Actor_Movie
                if (!context.Actors_Movies.Any())
                {
                    var inception = context.Movies.First(m => m.Title == "Inception");
                    var interstellar = context.Movies.First(m => m.Title == "Interstellar");

                    var leo = context.Actors.First(a => a.Name == "Leonardo DiCaprio");
                    var joseph = context.Actors.First(a => a.Name == "Joseph Gordon-Levitt");
                    var matthew = context.Actors.First(a => a.Name == "Matthew McConaughey");

                    context.Actors_Movies.AddRange(new List<Actor_Movie>
            {
                new Actor_Movie { MovieId = inception.Id, ActorId = leo.Id },
                new Actor_Movie { MovieId = inception.Id, ActorId = joseph.Id },
                new Actor_Movie { MovieId = interstellar.Id, ActorId = matthew.Id }
            });

                    context.SaveChanges();
                }

                // 6️⃣ Screenings
                if (!context.Screenings.Any())
                {
                    var inceptionId = context.Movies.First(m => m.Title == "Inception").Id;
                    var interstellarId = context.Movies.First(m => m.Title == "Interstellar").Id;
                    var duneId = context.Movies.First(m => m.Title == "Dune: Part One").Id;

                    var screenings = new List<Screening>
    {
        new Screening { MovieId = inceptionId, StartTime = DateTime.Now.AddHours(2), Price = 10 },
        new Screening { MovieId = interstellarId, StartTime = DateTime.Now.AddHours(4), Price = 12 },
        new Screening { MovieId = duneId, StartTime = DateTime.Now.AddHours(6), Price = 14 }
    };

                    context.Screenings.AddRange(screenings);
                    context.SaveChanges();
                }

            }


        }

        

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@etickets.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
