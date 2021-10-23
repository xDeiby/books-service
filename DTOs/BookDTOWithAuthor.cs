
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.DTOs
{
    public class BookDTOWithAuthor: BookDTO
    {
        public List<AuthorDTO> Authors { get; set; }
    }
}
