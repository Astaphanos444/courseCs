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
        Task<Comment?> getCommentById(int? id);
        Task<Comment?> CreateComment(Comment comment);
        Task<Comment?> UpdateComment(int? id,UpdateCommentDto updateDto);
        Task<Comment?> DeleteComment(int? id);
    }
}