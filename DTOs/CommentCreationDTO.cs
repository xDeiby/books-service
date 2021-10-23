using System.ComponentModel.DataAnnotations;

namespace WebApi2.DTOs
{
    public class CommentCreationDTO
    {
        [Required]
        public string Message { get; set; }
    }
}
