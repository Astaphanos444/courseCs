using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.CommentDto;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICommentRepository _commentRepository;
        public CommentController(ApplicationDBContext context, ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DtoComment>>> GetAllStocks()
        {
            var comments = await _commentRepository.getAllComments();

            var CommentsDto = comments.Select(comment => comment.CommentToCommentDto());

            return Ok(CommentsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById([FromRoute] int? id)
        {
            var comment = await _commentRepository.getCommentById(id);

            if (comment == null) return NotFound("Invalid Id");

            return Ok(comment.CommentToCommentDto());
        }

        [HttpPost]
        public async Task<ActionResult<DtoComment>> CreateStockRequest([FromBody] CreateCommentDto commentDto)
        {
            var comment = await _commentRepository.CreateComment(commentDto);

            if (comment == null) return NotFound();

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment.CommentToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Comment>> UpdateStock([FromRoute] int? id, [FromBody] UpdateCommentDto updateDto){

            var comment = await _commentRepository.UpdateComment(id, updateDto);

            if (comment == null) return NotFound();

            return Ok(comment.CommentToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Stock>> DeleteStock([FromRoute] int? id){
            var comment = await _commentRepository.DeleteComment(id);

            if(comment == null) return NotFound();

            return Ok();
        }
    }
}