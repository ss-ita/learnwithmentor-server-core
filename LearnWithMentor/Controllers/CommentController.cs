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
    /// <summary>
    /// Controller, that provides API for work with comments
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommentController : Controller
    {
        /// <summary>
        /// Services for work with different DB parts
        /// </summary>
        private readonly ICommentService commentService;

        /// <summary>
        /// Services initiation
        /// </summary>
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        /// <summary>Returns comment by id.</summary>
        /// <param name="id">Id of the comment.</param>
        [HttpGet]
        [Route("api/comment")]
        public async Task<ActionResult> GetCommentAsync(int id)
        {
            try
            {
                CommentDTO comment = await commentService.GetCommentAsync(id);
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

        /// <summary>Returns comments by plantask id.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
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


        /// <summary>Adds comment for planTask.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
        /// <param name="comment">New comment.</param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/comment")]
        public  async Task<ActionResult> PostAsync(int planTaskId, CommentDTO comment)
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

        /// <summary>Updates comment by id.</summary>
        /// <param name="commentId">Id of the comment.</param>
        /// <param name="comment">New comment.</param>
        [HttpPut]
        [Route("api/comment")]
        public async Task<ActionResult> PutCommentAsync(int commentId, [FromBody]CommentDTO comment)
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

        /// <summary>Deletes comment by id.</summary>
        /// <param name="commentId">Id of the comment.</param>
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

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            commentService.Dispose();
            base.Dispose(disposing);
        }
    }
}