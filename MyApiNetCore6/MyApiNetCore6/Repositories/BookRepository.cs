using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore6.Data;
using MyApiNetCore6.Models;
using MyApiNetCore6.Repositories;

namespace MyApiNetCore6.Helpers
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;
        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> addBookAsync(BookModel book)
        {
            var bookEntity = _mapper.Map<Book>(book);
            _context.Books!.Add(bookEntity);
            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task deleteBookAsync(int id)
        {
            var book = _context.Books!.Find(id);

            if(book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookModel>> getAllBooksAsync()
        {
            var listbooks = await _context.Books!.ToListAsync();
            return _mapper.Map<IEnumerable<BookModel>>(listbooks);
        }

        public async Task<BookModel> getBookByIdAsync(int id)
        {
            var book = await _context.Books!.FindAsync(id);
            return _mapper.Map<BookModel>(book);
        }

        public async Task<BookModel> getBookAsync(int id)
        {
            var book = await _context.Books!.FirstOrDefaultAsync(b => b.Id == id);
            return _mapper.Map<BookModel>(book);
        }

        public async Task updateBookAsync(int id, BookModel book)
        {
            if (id == book.Id)
            {
                var updatedBook = _mapper.Map<Book>(book);
                _context.Books!.Update(updatedBook); 

                await _context.SaveChangesAsync();
            }
        }
    }
}
