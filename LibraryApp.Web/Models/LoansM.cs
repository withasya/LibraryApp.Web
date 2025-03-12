using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Web.Models
{
    public class LoansM
    {
        public int Id { get; set; }  // Primary Key

        [Required]
        [ForeignKey("Books")]
        public int BooksId { get; set; }  // Hangi kitap ödünç alındı?

        [Required]
        [ForeignKey("Members")]
        public int MembersId { get; set; }  // Hangi üye aldı?

        // Kitap ve Üye ilişkisi (Navigation Property) - NULL OLAMAZ

        public BooksM? Books { get; set; } // Navigation property


        public MembersM? Members { get; set; } // Navigation property
    }
}
