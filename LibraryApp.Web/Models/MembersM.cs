namespace LibraryApp.Web.Models
{
    public class MembersM
    {
        public int Id { get; set; }  // Primary Key
        public required string Name { get; set; }  // Üye Adı
        public required string Email { get; set; }  // E-posta

        // Bir üyenin birden fazla ödünç alımı olabilir
        public virtual ICollection<LoansM> Loans { get; set; } = [];

    }
}
