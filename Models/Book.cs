using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApi2.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime? CreationDate { get; set; }

        public List<Comment> Comments { get; set; }

        public List<AuthorBook> AuthorsBooks { get; set; }
    }
}
