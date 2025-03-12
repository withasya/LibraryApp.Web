using AutoMapper;
using LibraryApp.Web.Dtos;
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
            CreateMap<BooksM, BooksM>().ForMember(dest => dest.Id, opt => opt.Ignore());


            // Üye eşlemeleri (Yeni ekledik)
            CreateMap<MemberDto, MembersM>();
            CreateMap<MembersM, MemberDto>();

            // Loan eşlemeleri
            CreateMap<LoansM, LoanDto>(); // LoansM'den LoanDTO'ya
            CreateMap<LoanDto, LoansM>(); // LoanDTO'dan LoansM'ye
        }
    }
}
