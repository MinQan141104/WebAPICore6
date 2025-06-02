using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApiNetCore6.Helpers;
using MyApiNetCore6.Models;
using MyApiNetCore6.Repositories;

namespace MyApiNetCore6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBookRepository _repo;
        public ProductsController(IBookRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize(Roles = AppRole.Customer)]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                return Ok(await _repo.getAllBooksAsync());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _repo.getBookByIdAsync(id);

            return book switch
            {
                not null => Ok(book),
                _ => NotFound()
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBook(BookModel book)
        {
            try
            {
                var newBookId = await _repo.addBookAsync(book);
                var bookResponse = await _repo.getBookAsync(newBookId);
                return bookResponse != null ? Ok(bookResponse) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatedBook(int id, [FromBody] BookModel book)
        {
            if(id != book.Id)
            {
                return NotFound();
            }
            try
            {
                await _repo.updateBookAsync(id, book);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var existBook = await _repo.getBookByIdAsync(id); 
                if (existBook == null)
                {
                    return NotFound();
                }

                await _repo.deleteBookAsync(id);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
