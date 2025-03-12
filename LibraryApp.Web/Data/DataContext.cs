using LibraryApp.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Web.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<BooksM> Books { get; set; }
        public DbSet<MembersM> Members { get; set; }
        public DbSet<LoansM> Loans { get; set; }

        // Lazy Loading Aktif!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); 
        }

        // Fluent API ile ilişkiler kurulacak
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // LoansM ile BooksM arasındaki ilişkiyi kur
            modelBuilder.Entity<LoansM>()
                .HasOne(l => l.Books)  // Bir ödünç işlemi bir kitaba aittir
                .WithMany()  // Kitap birçok ödünç işlemine sahip olabilir
                .HasForeignKey(l => l.BooksId)  // LoansM tablosundaki BooksId'yi kullan
                .OnDelete(DeleteBehavior.Restrict); // Silme davranışı

            // LoansM ile MembersM arasındaki ilişkiyi kur
            modelBuilder.Entity<LoansM>()
                .HasOne(l => l.Members)  // Bir ödünç işlemi bir üyeye aittir
                .WithMany(m => m.Loans)  // Üye birçok ödünç işlemine sahip olabilir
                .HasForeignKey(l => l.MembersId)  // LoansM tablosundaki MembersId'yi kullan
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
