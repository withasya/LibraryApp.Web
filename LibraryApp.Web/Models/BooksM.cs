namespace LibraryApp.Web.Models
{
    public class BooksM
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int YearPublished { get; set; }
        public bool IsAvailable { get; set; }
    }
}
