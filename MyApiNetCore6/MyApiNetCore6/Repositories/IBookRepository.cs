using MyApiNetCore6.Data;
using MyApiNetCore6.Models;

namespace MyApiNetCore6.Repositories
{
    public interface IBookRepository
    {
        public Task<IEnumerable<BookModel>> getAllBooksAsync();
        public Task<BookModel> getBookByIdAsync(int id);
        public Task<int> addBookAsync(BookModel book);
        public Task<BookModel> getBookAsync(int id);
        public Task updateBookAsync(int id, BookModel book);
        public Task deleteBookAsync(int id);
    }
}
