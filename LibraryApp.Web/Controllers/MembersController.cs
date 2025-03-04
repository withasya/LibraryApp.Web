using LibraryApp.Web.Data;
using LibraryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {

        private readonly DataContext _context;

        public MembersController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if( member == null)
            {
                return NotFound("Üye Bulunamadı");  // 404 hata kodu, çünkü üye bulunamadı
            }

            return Ok(member);  // 200 ile üye bilgilerini döndür
        }


        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] MembersM newMember)
        {
            if( newMember== null)
            {
                return BadRequest("Geçersiz Veri");
            }

            await _context.Members.AddAsync(newMember);
            await _context.SaveChangesAsync();


            // Başarıyla eklenen üyeyi döndür
            // CreatedAtAction, yeni oluşturulan kaydın bulunduğu URL'yi döndürür
            return CreatedAtAction(nameof(GetMember), new { id = newMember.Id }, newMember);
        }


    }
}
