using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.CommentDto
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Message must be over 5 char")]
        [MaxLength(280, ErrorMessage = "Message must be under 280 char")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Message must be over 5 char")]
        [MaxLength(280, ErrorMessage = "Message must be under 280 char")]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public int? StockId { get; set; }
    }
}