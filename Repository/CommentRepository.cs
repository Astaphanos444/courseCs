using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Comment?> CreateComment(CreateCommentDto createDto) 
        {
            var comment = createDto.CreateToComment();

            if(comment == null) return null;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> DeleteComment(long id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id) as Comment;

            if(comment == null) return null;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> getAllComments()
        {
            var comments = await _context.Comments.ToListAsync();

            return comments;
        }

        public async Task<Comment?> getCommentById(long id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            return comment;
        }

        public async Task<Comment?> UpdateComment(long id, UpdateCommentDto updateDto)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id) as Comment;

            if(comment == null) return null;

            comment.Title = updateDto.Title;
            comment.Content = updateDto.Content;

            await _context.SaveChangesAsync();

            return comment;
        }
    }
}