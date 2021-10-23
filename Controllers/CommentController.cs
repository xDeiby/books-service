using AutoMapper;
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
    [Route("api/books/{bookId:int}/comments")]
    public class CommentController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get([FromRoute] int bookId)
        {
            var comments = await context.Comments.Where(commentDb => commentDb.BookId == bookId).ToListAsync();

            return mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id:int}", Name = "GetComment")]
        public async Task<ActionResult<CommentDTOWithBook>> Get([FromRoute] int bookId, [FromRoute] int id)
        {
            var comment = await context.Comments.Include(commentDb => commentDb.Book).FirstOrDefaultAsync(commentDb => commentDb.Id == id && commentDb.BookId == bookId);

            if(comment == null)
            {
                return NotFound("Este libro no existe");
            }

            return mapper.Map<CommentDTOWithBook>(comment);
        }


        [HttpPost]
        public async Task<ActionResult<CommentDTO>> Post([FromRoute] int bookId, [FromBody] CommentCreationDTO commentCreation)
        {
            var existBook = await context.Books.AnyAsync(bookDb => bookDb.Id == bookId);

            if (existBook)
            {
                var comment = mapper.Map<Comment>(commentCreation);
                comment.BookId = bookId;
                context.Add(comment);
                await context.SaveChangesAsync();

                var commentDTO = mapper.Map<CommentDTO>(comment);

                return CreatedAtRoute("GetComment", new { BookId = comment.BookId , Id = comment.Id }, commentDTO);
            }

            return NotFound($"No existe el libro {bookId}");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CommentCreationDTO commentCreationDTO)
        {
            var exist = await context.Comments.AnyAsync(commentDb => commentDb.Id == id && commentDb.BookId == bookId);

            if (exist)
            {
                var comment = mapper.Map<Comment>(commentCreationDTO);
                comment.Id = id;
                comment.BookId = bookId;

                context.Update(comment);
                await context.SaveChangesAsync();
                return NoContent();
            }

            return NotFound();
        }
    }
}
