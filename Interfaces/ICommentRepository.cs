using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.CommentDto;

using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> getAllComments();
        Task<Comment?> getCommentById(long id);
        Task<Comment?> CreateComment(CreateCommentDto createDto);
        Task<Comment?> UpdateComment(long id,UpdateCommentDto updateDto);
        Task<Comment?> DeleteComment(long id);
    }
}