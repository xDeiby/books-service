using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2.Data;
using WebApi2.DTOs;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    [ApiController]
    [Route("/api/authors")]
    public class AuthorController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> Get()
        {
            var authors = await context.Authors.ToListAsync();
            return mapper.Map<List<AuthorDTO>>(authors);
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorDTOWithBook>> Get(int id)
        {
            var author = await context.Authors.Include(authorDb => 
                authorDb.AuthorsBooks).ThenInclude(authorBook => 
                    authorBook.Book).FirstOrDefaultAsync(authorDb => authorDb.Id == id);

            if (author == null)
            {
                return NotFound($"No se encontro el author con id {id}");
            }

            return mapper.Map<AuthorDTOWithBook>(author);
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<List<AuthorDTO>>> Get(string name)
        {
            var authors = await context.Authors.Where(authorDb => authorDb.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AuthorDTO>>(authors);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> Post(AuthorCreationDTO authorCreation)
        {
            var repeatName = await context.Authors.AnyAsync(currentAuthor => currentAuthor.Name == authorCreation.Name);

            if (repeatName)
            {
                return BadRequest($"Ya existe un author con el nombre {authorCreation.Name}");
            }

            var author = mapper.Map<Author>(authorCreation);

            context.Add(author);
            await context.SaveChangesAsync();

            var authorDTO = mapper.Map<AuthorDTO>(author);

            return CreatedAtRoute("GetAuthor", new { Id = author.Id }, authorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AuthorCreationDTO authorCreationDTO, int id)
        {
            var existAuthor = await context.Authors.AnyAsync(currentAuthor => currentAuthor.Id == id);
            if (existAuthor)
            {
                var author = mapper.Map<Author>(authorCreationDTO);
                author.Id = id;

                context.Update(author);
                await context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound($"No existe ningun author con el id {id}");
        }
         
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var validId = await context.Authors.AnyAsync(author => author.Id == id);

            if (validId)
            {
                context.Remove(new Author() { Id = id });
                await context.SaveChangesAsync();
                return NoContent();
            }

            return NotFound($"No existe ningun author con el id {id}");
        }
    }
}
