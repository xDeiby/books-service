using System.ComponentModel.DataAnnotations;

namespace WebApi2.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int BookId { get; set; }

        //Navigation Property
        public Book Book { get; set; }
    }
}
