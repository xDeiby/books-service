using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2.DTOs;
using WebApi2.Models;

namespace WebApi2.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthorCreationDTO, Author>();

            CreateMap<Author, AuthorDTO>();

            CreateMap<Author, AuthorDTOWithBook>()
                .ForMember(authorDTO => authorDTO.Books, options => options.MapFrom(MapAuthorDTOBooks));

            //Configuramos el mapeo hacia la propiedad AuthorsBooks
            CreateMap<BookCreationDTO, Book>()
                .ForMember(book => book.AuthorsBooks, options => options.MapFrom(MapAuthorBook));

            CreateMap<Book, BookDTO>();

            //Reverse permite el mapeo contrario
            CreateMap<Book, BookPatchDTO>().ReverseMap();

            CreateMap<Book, BookDTOWithAuthor>()
                .ForMember(bookDTO => bookDTO.Authors, options => options.MapFrom(MapAuthorDTO));

            CreateMap<CommentCreationDTO, Comment>();

            CreateMap<Comment, CommentDTOWithBook>();

            CreateMap<Comment, CommentDTO>();

        }

        private List<BookDTO> MapAuthorDTOBooks(Author author, AuthorDTO authorDTO)
        {
            var result = new List<BookDTO>();

            if(author.AuthorsBooks == null) { return result; }

            foreach (var authorBook in author.AuthorsBooks)
            {
                result.Add(new BookDTO() { 
                    Id = authorBook.Book.Id,
                    Title = authorBook.Book.Title
                });
            }

            return result;
        }

        private List<AuthorDTO> MapAuthorDTO(Book book, BookDTO bookDTO)
        {
            var result = new List<AuthorDTO>();

            foreach (var authorBook in book.AuthorsBooks)
            {
                result.Add(new AuthorDTO()
                {
                    Id = authorBook.AuthorId,
                    Name = authorBook.Author.Name
                });
            }

            return result;
        }

        private List<AuthorBook> MapAuthorBook(BookCreationDTO bookCreationDTO, Book book)
        {
            var result = new List<AuthorBook>();

            foreach (var authorId in bookCreationDTO.AuthorIds)
            {
                //.Net se encarga de agregar el libro
                result.Add(new AuthorBook() { AuthorId = authorId });
            }

            return result;
        }
    }
}
