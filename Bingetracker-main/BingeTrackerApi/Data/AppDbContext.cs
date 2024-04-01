using BingeTrackerApi.Model;
using Humanizer.Localisation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BingeTrackerApi.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Streaming> Streamings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=KutLuna;" +
                "database=BingeTracker;",
            ServerVersion.Parse("5.7.33-winx64")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Streaming>().HasData(
                new Streaming { Id = 1, Title = "Spiderman", Platform = "Netflix", Genre = "Fiction", ReleaseDate = new DateOnly(2023, 12, 02), Rating = 5 },
                new Streaming { Id = 2, Title = "The seven Deadly Sins", Platform = "Netflix", Genre = "Anime", ReleaseDate = new DateOnly(2014, 10, 05), Rating = 5 },
                new Streaming { Id = 3, Title = "Jujustu Kaisen", Platform = "Crunchyroll", Genre = "Anime", ReleaseDate = new DateOnly(2018, 03, 05), Rating = 5 },
               new Streaming { Id = 4, Title = "Attack on Titan", Platform = "Crunchyroll", Genre = "Anime", ReleaseDate = new DateOnly(2013, 04, 07), Rating = 4 },
               new Streaming { Id = 5, Title = "Breaking Bad", Platform = "Netflix", Genre = "Drama", ReleaseDate = new DateOnly(2008, 01, 20), Rating = 2 },
               new Streaming { Id = 7, Title = "Coach Carter", Platform = "Hulu", Genre = "Drama", ReleaseDate = new DateOnly(2005, 01, 14), Rating = 3 },
               new Streaming { Id = 8, Title = "Arcane", Platform = "Netflix", Genre = "Animation", ReleaseDate = new DateOnly(2021, 11, 06), Rating = 5 },
               new Streaming { Id = 9, Title = "The Mandalorian", Platform = "Disney+", Genre = "Science Fiction", ReleaseDate = new DateOnly(2019, 11, 12), Rating = 2 },
               new Streaming { Id = 10, Title = "Friends", Platform = "Netflix", Genre = "Comedy", ReleaseDate = new DateOnly(1994, 09, 22), Rating = 4 });
        }
    }
}
