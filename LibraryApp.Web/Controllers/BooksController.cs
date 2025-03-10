﻿using LibraryApp.Web.Data;
using LibraryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using LibraryApp.Web.Profiles;

namespace LibraryApp.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BooksController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var bookList = _context.Books.ToList();     // Veritabanından tüm kitapları al  //bookList değişkendir
            return Ok(bookList);                        // 200 OK ile geri döndür
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] BooksM newBook)
        {
            if (newBook == null) //kitap var mı kontrol
            {
                return BadRequest("Kitap verisi geçersiz.");
            }

            _context.Books.Add(newBook); //ekleme
            _context.SaveChanges();     //kayıt

            return CreatedAtAction(nameof(GetAllBooks), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BooksM updatedBook)
        {
            // Kitabı ID'ye göre bulalım
            var existingBook = _context.Books.FirstOrDefault(b => b.Id == id);

            // Kitap bulunmazsa
            if (existingBook == null)
            {
                return NotFound("Kitap Bulunamadı"); // 404 döndür
            }

            // AutoMapper kullanarak existingBook'u güncelle
            _mapper.Map(updatedBook, existingBook);  // updatedBook'tan existingBook'a veri kopyalanacak

            _context.SaveChanges();  // Güncellemeleri kaydet

            return Ok(existingBook);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            // Kitap ID'ye göre veritabanında aranır
            var bookToDelete = _context.Books.FirstOrDefault(b => b.Id == id);

            // Kitap bulunamazsa 404 Not Found döndürülür
            if (bookToDelete == null)
            {
                return BadRequest("Kitap Bulunamadı");
            }

            // Kitap bulunursa silinir
            _context.Books.Remove(bookToDelete);

            _context.SaveChanges();

            // Başarıyla silindiğinde NoContent (204) döner
            return NoContent();
        }


    }
}

