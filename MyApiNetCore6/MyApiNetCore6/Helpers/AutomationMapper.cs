using AutoMapper;
using MyApiNetCore6.Data;
using MyApiNetCore6.Models;

namespace MyApiNetCore6.Helpers
{
    public class AutomationMapper : Profile
    {
        public AutomationMapper()
        {
            CreateMap<Book, BookModel>()
                .ReverseMap();
        }
    }
}
