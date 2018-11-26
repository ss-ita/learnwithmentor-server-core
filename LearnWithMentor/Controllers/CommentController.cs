using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LearnWithMentor.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        [Route("api/comment")]
        public async Task<ActionResult> GetCommentAsync(int id)
        {
            try
            {
                CommentDto comment = await commentService.GetCommentAsync(id);
                if (comment == null)
                {
                    return NoContent();
                }
                return Ok(comment);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/comment/plantask/{planTaskId}")]
        public async Task<ActionResult> GetCommentsForPlanTaskAsync(int planTaskId)
        {
            try
            {
                var comments = await commentService.GetCommentsForPlanTaskAsync(planTaskId);
                if (comments == null)
                {
                    return NoContent();
                }
                return Ok(comments);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    
        [AllowAnonymous]
        [HttpPost]
        [Route("api/comment")]
        public  async Task<ActionResult> PostAsync(int planTaskId, CommentDto comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if ( await commentService.AddCommentToPlanTaskAsync(planTaskId, comment))
                {
                    var log = $"Succesfully created comment with id = {comment.Id} by user id = {comment.CreatorId}";
                    return Ok(log);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("api/comment")]
        public async Task<ActionResult> PutCommentAsync(int commentId, [FromBody]CommentDto comment)
        {
            try
            {
                if (await commentService.UpdateCommentAsync(commentId, comment))
                {
                    var log = $"Succesfully updated comment with id = {commentId}";
                    return Ok(log);
                }
                return NoContent(); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete]
        [Route("api/comment/{commentId}")]
        public async Task<ActionResult> DeleteAsync(int commentId)
        {
            try
            {
                if (await commentService.RemoveByIdAsync(commentId))
                {
                    var log = $"Succesfully deleted comment with id = {commentId}";
                    return Ok(log);
                }
                var message = "Not possibly to delete comment: comment does not exist.";
                return BadRequest(message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            commentService.Dispose();
            base.Dispose(disposing);
        }
    }
}