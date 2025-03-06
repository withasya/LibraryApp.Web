using LibraryApp.Web.Data;
using LibraryApp.Web.Models;
using LibraryApp.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Execution;
using AutoMapper;

namespace LibraryApp.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MembersController(DataContext context, IMapper mapper) : ControllerBase
    {

        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper; // 🟢 AutoMapper Enjeksiyonu

        [HttpGet]
        public async Task<IActionResult> GetAllMember()
        {
            var allMembers = await _context.Members.ToListAsync();

            if (allMembers == null || allMembers.Count == 0)
            {
                return NotFound("hic üye yok");
            }

            return Ok(allMembers);
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
        public async Task<IActionResult> AddMember([FromBody] MemberDto newMember)
        {

            if (string.IsNullOrWhiteSpace(newMember.Name) || string.IsNullOrWhiteSpace(newMember.Email))
            {
                return BadRequest("Ad ve Email boş olamaz.");  // 400 Bad Request
            }

            // Yeni üye oluşturuyoruz (veritabanı modeline dönüştürüyoruz)
            var member = new MembersM
            {
                Name = newMember.Name,
                Email = newMember.Email,
            };


            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();


            // Başarıyla eklenen üyeyi döndür
            // CreatedAtAction, yeni oluşturulan kaydın bulunduğu URL'yi döndürür
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, newMember);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, MemberDto updatedMemberDto)
        {
            var existingMember = await _context.Members.FindAsync(id);

            if (existingMember == null)
            {
                return NotFound("Böyle Bir Üye Bulunmamaktadır");
            }

            // 🟢 AutoMapper ile Güncelleme
            _mapper.Map(updatedMemberDto, existingMember);

            await _context.SaveChangesAsync();
            return Ok(existingMember); //Ancak çoğu REST API'de güncelleme işlemlerinde 204 "No Content" kullanılır.

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var existingMember = await _context.Members.FindAsync(id);
            if (existingMember == null)
            {
                return NotFound("Böyle Bir Üye Bulunmamaktadır");
            }

            _context.Members.Remove(existingMember);
            await _context.SaveChangesAsync();

            return NoContent(); //204 no content (başarıyla silindi)


        }

    }
}
