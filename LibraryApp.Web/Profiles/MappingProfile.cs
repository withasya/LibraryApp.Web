using AutoMapper;
using LibraryApp.Web.Models;

namespace LibraryApp.Web.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // BooksM türü için kopyalama yapılacak
            // BooksM'den BooksM'ye kopyalama
            CreateMap<BooksM, BooksM>();
        }
    }
}
