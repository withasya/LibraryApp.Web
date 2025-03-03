namespace LibraryApp.Web.Models
{
    public class LoansM
    {
            public int Id { get; set; }  // Primary Key
            public int BooksId { get; set; }  // Hangi kitap ödünç alındı?
            public int MembersId { get; set; }  // Hangi üye aldı?

            // Kitap ve Üye ilişkisi (Navigation Property)
            public BooksM Books { get; set; }
            public MembersM Members { get; set; }
    }
}
