using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.CommentDto;
using api.Extentions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        public CommentController(ApplicationDBContext context, ICommentRepository commentRepository, UserManager<AppUser> userManager, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _context = context;
            _userManager = userManager;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<DtoComment>>> GetAllStocks()
        {
            var comments = await _commentRepository.getAllComments();

            var CommentsDto = comments.Select(comment => comment.CommentToCommentDto());

            return Ok(CommentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Comment>> GetCommentById([FromRoute] int? id)
        {
            var comment = await _commentRepository.getCommentById(id);

            if (comment == null) return NotFound("Invalid Id");

            return Ok(comment.CommentToCommentDto());
        }

        [HttpPost]
        public async Task<ActionResult<DtoComment>> CreateStockRequest([FromBody] CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            if(!await _stockRepository.StockExists(commentDto.StockId)) return BadRequest("Stock doesn't exist");

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var comment = commentDto.CreateToComment();
            comment.AppUserId = appUser.Id;
            await _commentRepository.CreateComment(comment);

            if (comment == null) return NotFound();

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment.CommentToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Comment>> UpdateStock([FromRoute] int? id, [FromBody] UpdateCommentDto updateDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            var comment = await _commentRepository.UpdateComment(id, updateDto);

            if (comment == null) return NotFound();

            return Ok(comment.CommentToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Stock>> DeleteStock([FromRoute] int? id){
            var comment = await _commentRepository.DeleteComment(id);

            if(comment == null) return NotFound();

            return Ok();
        }
    }
}