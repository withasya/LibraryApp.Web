using LibraryApp.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Web.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<BooksM> Books { get; set; }

    }
}

