using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if (environment.IsDevelopment())
                {
                    context.Difficulties.Add(new Difficulty {Id = "easy", Name = "Easy", Description = "Easy test"});
                    context.Difficulties.Add(new Difficulty {Id = "medium", Name = "Medium", Description = "Medium test"});
                    context.Difficulties.Add(new Difficulty {Id = "hard", Name = "Hard", Description = "Hard test"});
                    context.Categories.Add(new Category {Id = "kick", Name = "Kick", Description = "Kick test"});
                    context.Categories.Add(new Category {Id = "flip", Name = "Flip", Description = "Flip test"});
                    context.Categories.Add(new Category {Id = "trans", Name = "Transition", Description = "Transition test"});

                    context.Add(new Trick
                    {
                        Id = "backwards-roll", 
                        Name = "Backwards roll",
                        Description = "This is a test roll", 
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory> {new () {CategoryId = "flip"}}
                    });
                    
                    context.Add(new Trick
                    {
                        Id = "back-flip", 
                        Name = "Back flip",
                        Description = "This is a test flip", 
                        Difficulty = "medium",
                        TrickCategories = new List<TrickCategory> {new () {CategoryId = "flip"}},
                        Prerequisites = new List<TrickRelationship>
                        {
                            new() { PrerequisiteId = "backwards-roll"}
                        }
                    });

                    /*context.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Going for max height",
                        Video = "file_example_MP4_1280_10MG.mp4",
                        VideoProcessed = true
                    });
                    
                    context.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Going for min height",
                        Video = "file_example_MP4_1280_10MG.mp4",
                        VideoProcessed = true
                    });*/

                    context.SaveChanges();
                }
            }
            
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}