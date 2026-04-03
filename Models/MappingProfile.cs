using AutoMapper;
using FirstApi.DTOs.Book;
using FirstApi.Models;

namespace FirstApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.FullName));
    }
}
