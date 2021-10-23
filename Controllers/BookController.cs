using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2.Data;
using WebApi2.DTOs;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BookController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> Get()
        {
            var books = await context.Books.ToListAsync();

            return mapper.Map<List<BookDTO>>(books);
        }

        [HttpGet("{id:int}", Name = "GetBook")]
        public async Task<ActionResult<BookDTOWithAuthor>> Get(int id)
        {
            var book = await context.Books.Include(bookDb => bookDb.AuthorsBooks).ThenInclude(authorBook => authorBook.Author).FirstOrDefaultAsync(bookDB => bookDB.Id == id);

            book.AuthorsBooks = book.AuthorsBooks.OrderBy(authorBook => authorBook.Order).ToList();

            return mapper.Map<BookDTOWithAuthor>(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> Post(BookCreationDTO bookCreation)
        {

            if(bookCreation.AuthorIds == null) { return BadRequest(); }

            var authorIds = await context.Authors.Where(authorDb => bookCreation.AuthorIds.Contains(authorDb.Id)).Select(authorDb => authorDb.Id).ToListAsync();

            if (bookCreation.AuthorIds.Count != authorIds.Count)
            {
                return NotFound("Uno de los autores no existe");
            }

            var book = mapper.Map<Book>(bookCreation);

            for (int i = 0; i < book.AuthorsBooks.Count; i++)
            {
                book.AuthorsBooks[i].Order = i;
            }

            context.Add(book);
            await context.SaveChangesAsync();

            var bookDTO = mapper.Map<BookDTO>(book);

            return CreatedAtRoute("GetBook", new { Id = book.Id }, bookDTO);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existId = await context.Books.AnyAsync(book => book.Id == id);

            if (existId)
            {
                context.Remove(new Book() { Id = id });
                await context.SaveChangesAsync();
                return NoContent();
            }

            return NotFound($"No se encontro un libro con el id {id}");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, BookCreationDTO bookCreactionDTO)
        {
            //var authorExist = await context.Authors.Where(authorDb => bookCreactionDTO.AuthorIds.Contains(authorDb.Id)).ToListAsync();
            var book = await context.Books.Include(bookDb => bookDb.AuthorsBooks).FirstOrDefaultAsync(bookDb => bookDb.Id == id);

            //if(authorExist.Count != bookCreactionDTO.AuthorIds.Count)
            if (book == null) { return NotFound(); }

            //Como book guarda la referencia al dato en la base datos, la asignacion se hace por referencia
            //Mapper => pasa los datos de bookCreationDTO a book
            book = mapper.Map(bookCreactionDTO, book);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
        {
            //Si la informacion esta en el formato incorrecto
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var book = await context.Books.FirstOrDefaultAsync(bookDb => bookDb.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var bookDTO = mapper.Map<BookPatchDTO>(book);

            //Se realizan los cambios que vienen en el patchDocumente, en el libro, si hay algun error, pasan al ModelState
            patchDocument.ApplyTo(bookDTO, ModelState);


            var isValid = TryValidateModel(bookDTO);

            if (!isValid)
            {
                //Se retornan los errores de validacion encontrados
                return BadRequest(ModelState);
            }

            //Si es valido, se hace la modificacion
            mapper.Map(bookDTO, book);
            await context.SaveChangesAsync();

            return NoContent();

            
        }

    }
}
