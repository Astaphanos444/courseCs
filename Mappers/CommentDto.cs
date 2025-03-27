using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentDto
    {
        public static Comment CreateToComment(this CreateCommentDto createDto)
        {
            return new Comment
            {
                Title = createDto.Title,
                Content = createDto.Content,
                CreatedOn = createDto.CreatedOn,
                StockId = createDto.StockId,
            };
        }
    }
}