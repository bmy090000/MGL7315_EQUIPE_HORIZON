using MaBiblio.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MaBiblio.DAL
{
    public class BiblioContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
       
        public DbSet<Author> Authors { get; set; }


    }
}