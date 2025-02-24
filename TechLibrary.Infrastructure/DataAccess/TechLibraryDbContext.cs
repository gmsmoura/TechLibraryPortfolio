using Microsoft.EntityFrameworkCore;
using System;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Infrastructure.DataAccess
{
    public class TechLibraryDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\gui_m\\Documents\\Projetos\\Back End\\TechLibrary\\Database\\TechLibraryDb.db");
        }
    }
}
