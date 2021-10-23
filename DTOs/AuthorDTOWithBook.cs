using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.DTOs
{
    public class AuthorDTOWithBook: AuthorDTO
    {
        public List<BookDTO> Books { get; set; }
    }
}
