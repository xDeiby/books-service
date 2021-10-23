using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.DTOs
{
    public class BookPatchDTO
    {
        public string Title { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
