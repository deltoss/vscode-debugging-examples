using System;
using System.Linq;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
{
    public static class DataInitialiser
    {
        /// <summary>
        /// Custom manual seeding approach.
        /// For other approaches or more information, see:
        ///   https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<BloggingContext>();
            IRelationalDatabaseCreator relationalDbCreator = context.Database.GetService<IRelationalDatabaseCreator>();
            // Apply migrations if the database exists
            //   https://www.ryadel.com/en/entity-framework-core-migrations-error-database-already-exists-fix-migrate-ef-dotnet/
            //   https://stackoverflow.com/questions/33911316/entity-framework-core-how-to-check-if-database-exists
            if (relationalDbCreator.Exists())
            {
                context.Database.Migrate(); // Create the Db if it doesn't exist and applies any existing pending migrations.
                SeedData(serviceProvider);
            }
        }

        public static void SeedData(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<BloggingContext>();
            if (!context.Blogs.Any())
            {
                context.Blogs.Add(new Blog() { BlogId = 1, Url = "blogexamples.com.au/1" });
                context.Blogs.Add(new Blog() { BlogId = 2, Url = "blogexamples.com.au/2" });
            }
            if (!context.Posts.Any())
            {
                context.Posts.Add(new Post() { PostId = 1, Title = "Post 1", Content = "Hello World.", BlogId = 1 });
                context.Posts.Add(new Post() { PostId = 2, Title = "Post 2", Content = "This is example data from database.", BlogId = 1 });
                context.Posts.Add(new Post() { PostId = 3, Title = "Post 3", Content = "So... now what?", BlogId = 2 });
                context.Posts.Add(new Post() { PostId = 4, Title = "Post 4", Content = "This is a very short post...", BlogId = 2 });
            }
            context.SaveChanges();
        }
    }
}