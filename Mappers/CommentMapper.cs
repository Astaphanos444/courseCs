using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.CommentDto;
using api.Models;
using Microsoft.AspNetCore.Components.Web;

namespace api.Mappers
{
    public static class CommentMapper
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

        public static DtoComment CommentToCommentDto(this Comment comment)
        {
            return new DtoComment
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
            };
        }
    }
}