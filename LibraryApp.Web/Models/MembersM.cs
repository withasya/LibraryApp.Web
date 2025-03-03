namespace LibraryApp.Web.Models
{
    public class MembersM
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Üye Adı
        public string Email { get; set; }  // E-posta

        // Bir üyenin birden fazla ödünç alımı olabilir
        public ICollection<LoansM> Loans { get; set; }
    }
}
