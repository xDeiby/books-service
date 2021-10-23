using System;
using System.Collections.Generic;


namespace WebApi2.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? CreationDate { get; set; }

        //public List<AuthorDTO> Authors { get; set; }

        //public List<CommentDTO> Comments { get; set; }
    }
}
