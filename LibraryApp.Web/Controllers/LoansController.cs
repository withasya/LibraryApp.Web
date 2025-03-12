using Microsoft.AspNetCore.Mvc;
using LibraryApp.Web.Data;
using LibraryApp.Web.Models;
using LibraryApp.Web.Dtos;
using MicrosoftAppBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper; // AutoMapper enjekte edildi

        // Constructor ile IMapper'ı enjekte ediyoruz
        public LoansController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; // Burada 'mapper' ifadesi doğru olmalı, IMapper interface'i üzerinden alıyoruz
        }

        [HttpPost]
        public async Task<ActionResult> AddLoan([FromBody] LoansM newLoan)
        {
            if (newLoan == null)
            {
                return BadRequest("Yeni ödünç kaydı geçersiz."); // 400 Bad Request
            }

            // Kitap ve Üye varlığını kontrol et
            var book = await _context.Books.FindAsync(newLoan.BooksId);
            var member = await _context.Members.FindAsync(newLoan.MembersId);

            if (book == null)
            {
                return NotFound($"Kitap bulunamadı. ID: {newLoan.BooksId}");
            }
            if (member == null)
            {
                return NotFound($"Üye bulunamadı. ID: {newLoan.MembersId}");
            }

            // Yeni ödünç kaydını oluştur
            var loan = new LoansM
            {
                BooksId = newLoan.BooksId,
                MembersId = newLoan.MembersId
                // Books ve Members alanlarını doldurmayın
            };

            // Loans tablosuna ekle
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            // Başarıyla eklenen ödünç kaydını döndür
            return CreatedAtAction(nameof(AddLoan), new { id = loan.Id }, new { loan.Id, loan.BooksId, loan.MembersId }); // 201 Created
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoansM>>> GetAllLoans()
        {
            //eager loading örneğidir (istekli yükleme)
            var loans = await _context.Loans
                .Include(l => l.Books)
                .Include(l => l.Members)
                .ToListAsync();

            return Ok(loans);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoan(int id)
        {
            //lazy loading örneğidir sadece loans tablosu verisi gelir eğer istersek diğer tablolar için yeni dorgu gerekir
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
                return NotFound("bu id bulunamadı");

            return Ok(loan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanDto updatedLoanDto)
        {
            var existingLoan = await _context.Loans.FindAsync(id);

            if (existingLoan == null) return NotFound("Bu ID'ye ait ödünç kaydı bulunamadı.");

            // AutoMapper kullanarak LoanDTO'yu mevcut Loan modeline dönüştürüyoruz
            _mapper.Map(updatedLoanDto, existingLoan);

            await _context.SaveChangesAsync();
            return Ok(existingLoan);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var deletedLoan =await _context.Loans.FindAsync(id);

            if (deletedLoan == null) return NotFound("Bu ID'ye ait ödünç kaydı bulunamadı.");
        

            _context.Loans.Remove(deletedLoan);
            await _context.SaveChangesAsync();

            return Ok(deletedLoan);
        }



        //TESTLER

        [HttpGet("test-loan")]
        public IActionResult TestLazyLoading([FromServices] DataContext _context)
        {
            var loan = _context.Loans.FirstOrDefault(); // İlk Loans kaydını alıyoruz

            if (loan == null)
            {
                return NotFound("Loans tablosunda kayıt bulunamadı.");
            }

            return Ok(new
            {
                LoanID = loan.Id,
                Book = loan.Books?.Title, // Eğer Lazy Loading çalışıyorsa, burası otomatik dolar!
                Member = loan.Members?.Name // Lazy Loading aktifse, üye bilgisi de gelir.
            });
        }

        //lazy test
        [HttpGet("lazy-loans")]
        public async Task<ActionResult<IEnumerable<LoansM>>> GetAllLoansLazy()
        {
            var loans = await _context.Loans.ToListAsync(); // İlişkili verileri çağırmıyoruz

            // Kitap ve üye bilgileri ilk aşamada yüklenmez, Lazy Loading devreye girecek
            return Ok(loans);
        }

        //eager test
        [HttpGet("eager-loans")]
        public async Task<ActionResult<IEnumerable<LoansM>>> GetAllLoan()
        {
            var loans = await _context.Loans
                .Include(l => l.Books)   // Kitapları baştan yükle
                .Include(l => l.Members) // Üyeleri baştan yükle
                .ToListAsync();

            return Ok(loans);
        }

        //As no tracking eager
        //Eğer veriler üzerinde değişiklik yapacaksanız, AsNoTracking kullanamazsınız çünkü:SaveChanges, değişiklikleri kaydetmek için izleme gerektirir.
        [HttpGet("eager-loans-no-tracking")]
        public async Task<ActionResult<IEnumerable<LoansM>>> GetAllLoansNoTracking()
        {
            var loans = await _context.Loans
                .AsNoTracking() // Veriyi takip etme, sadece oku!
                .Include(l => l.Books)
                .Include(l => l.Members)
                .ToListAsync();

            return Ok(loans);
        }


    }
}
