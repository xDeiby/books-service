using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2.Models;

namespace WebApi2.Data
{
    public class ApplicationDbContext: DbContext
    {
        private readonly DbContextOptions options;

        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            this.options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Se crea una llave primaria compuesta, mediante el author y libro
            modelBuilder.Entity<AuthorBook>().HasKey(al => new { al.AuthorId, al.BookId });
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuthorBook>  AuthorsBooks { get; set; }
    }
}
