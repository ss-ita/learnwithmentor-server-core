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
    [Authorize]
    public class CommentController : ApiController
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
        public async Task<HttpResponseMessage> GetCommentAsync(int id)
        {
            try
            {
                CommentDto comment = await commentService.GetCommentAsync(id);
                if (comment == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Comment with this Id does not exist in database.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, comment);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        /// <summary>Returns comments by plantask id.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
        [HttpGet]
        [Route("api/comment/plantask/{planTaskId}")]
        public async Task<HttpResponseMessage> GetCommentsForPlanTaskAsync(int planTaskId)
        {
            try
            {
                var comments = await commentService.GetCommentsForPlanTaskAsync(planTaskId);
                if (comments == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no comments for this task in that plan");
                }
                return Request.CreateResponse(HttpStatusCode.OK, comments);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        /// <summary>Adds comment for planTask.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
        /// <param name="comment">New comment.</param>
        [HttpPost]
        [Route("api/comment")]
        public  async Task<HttpResponseMessage> PostAsync(int planTaskId, CommentDto comment)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                if ( await commentService.AddCommentToPlanTaskAsync(planTaskId, comment))
                {
                    var log = $"Succesfully created comment with id = {comment.Id} by user id = {comment.CreatorId}";
                    return Request.CreateResponse(HttpStatusCode.OK, "Comment succesfully created");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Not possibly to add comment: task in this plan does not exist");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal creation error");
            }
        }

        /// <summary>Updates comment by id.</summary>
        /// <param name="commentId">Id of the comment.</param>
        /// <param name="comment">New comment.</param>
        [HttpPut]
        [Route("api/comment")]
        public async Task<HttpResponseMessage> PutCommentAsync(int commentId, [FromBody]CommentDto comment)
        {
            try
            {
                if (await commentService.UpdateCommentAsync(commentId, comment))
                {
                    var log = $"Succesfully updated comment with id = {commentId}";
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated comment id: {commentId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, ("Not possibly to update comment: comment does not exist."));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal updation error");
            }
        }

        /// <summary>Deletes comment by id.</summary>
        /// <param name="commentId">Id of the comment.</param>
        [HttpDelete]
        [Route("api/comment/{commentId}")]
        public async Task<HttpResponseMessage> DeleteAsync(int commentId)
        {
            try
            {
                if (await commentService.RemoveByIdAsync(commentId))
                {
                    var log = $"Succesfully deleted comment with id = {commentId}";
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted comment id: {commentId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, ("Not possibly to delete comment: comment does not exist."));
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal deletion error.");
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
