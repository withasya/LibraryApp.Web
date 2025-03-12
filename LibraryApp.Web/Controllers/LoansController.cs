using Microsoft.AspNetCore.Mvc;
using LibraryApp.Web.Data;
using LibraryApp.Web.Models;
using LibraryApp.Web.Dtos;
using MicrosoftAppBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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

            var loans = await _context.Loans
                .Include(l => l.Books)
                .Include(l => l.Members)
                .ToListAsync();

            return Ok(loans);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoan(int id)
        {
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
    }
}
