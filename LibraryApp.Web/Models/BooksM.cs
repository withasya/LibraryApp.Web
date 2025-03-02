namespace LibraryApp.Web.Models
{
    public class BooksM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearPublished { get; set; }
        public bool IsAvailable { get; set; }
    }
}
