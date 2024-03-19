using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.core;

namespace MovieDatabase.Infrastructure
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            var g1 = new Genre { Id = 1, Name = "Comedy" };

            var g2 = new Genre { Id = 2, Name = "Romance" };

            var g3 = new Genre { Id = 3, Name = "Thriller" };

            var g4 = new Genre { Id = 4, Name = "Drama" };

            var g5 = new Genre { Id = 5, Name = "Neo-Noir" };

            var g6 = new Genre { Id = 6, Name = "Action" };


            modelBuilder.Entity<Genre>().HasData(
                g1);

            modelBuilder.Entity<Genre>().HasData(
                g2);

            modelBuilder.Entity<Genre>().HasData(
                g3);

            modelBuilder.Entity<Genre>().HasData(
                g4);

            modelBuilder.Entity<Genre>().HasData(
                g5);

            modelBuilder.Entity<Genre>().HasData(
                g6);


            var m1 = new Movie
            {
                Id = 1,
                Name = "Vinnathandi Varuvaiya",
                Description = "A young, aspiring filmmaker falls in love with a girl who moves into the house above his, " +
                                  "but her sophisticated nature does more harm than good to him",
                rating = 8.1,
            };

            var m2 = new Movie
            {
                Id = 2,
                Name = "Vikram Vedha",
                Description = "A gritty crime thriller exploring the moral conflict between a police officer and a gangster",
                rating = 8.6,

            };

            var m3 = new Movie
            {
                Id = 3,
                   Name = "Super Deluxe",
                   Description = "A dark comedy-drama intertwining multiple stories of identity and acceptance in Chennai",
                   rating = 8.4,

            };

            var m4 = new Movie
            {
                Id = 4,
                Name = "Asuran",
                Description = "A raw and intense action drama depicting the struggle of a oppressed family against the tyranny of a powerful landlord.",
                rating = 8.5,


            };

            modelBuilder.Entity<Movie>().HasData(m1
                );

            modelBuilder.Entity<Movie>().HasData(m2
               );

            modelBuilder.Entity<Movie>().HasData(m3
               );

            modelBuilder.Entity<Movie>().HasData(m4
               );


            modelBuilder.Entity<MovieGenre>().HasData(
                new MovieGenre { MovieId = m1.Id, GenreId = g2.Id},
                new MovieGenre { MovieId = m2.Id,  GenreId = g6.Id},
                new MovieGenre { MovieId = m3.Id, GenreId = g5.Id},
                new MovieGenre { MovieId = m4.Id, GenreId = g4.Id }

               );


            modelBuilder.Entity<MovieGenre>().HasData(
                new MovieGenre { MovieId = m1.Id, GenreId = g4.Id },
                new MovieGenre { MovieId = m2.Id, GenreId = g3.Id },
                new MovieGenre { MovieId = m3.Id, GenreId = g6.Id },
                new MovieGenre { MovieId = m4.Id, GenreId = g2.Id }

               );
        }
    }
}
