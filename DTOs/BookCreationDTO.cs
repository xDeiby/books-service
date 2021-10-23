using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.DTOs
{
    public class BookCreationDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public List<int> AuthorIds { get; set; }

    }
}
