using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.Models
{
    public class AuthorBook
    {
        public int AuthorId { get; set; }

        public int BookId { get; set; }

        //Orden de los autores
        public int Order { get; set; }

        //Navigation Properties
        public Author Author { get; set; }

        public Book Book { get; set; }
    }
}
