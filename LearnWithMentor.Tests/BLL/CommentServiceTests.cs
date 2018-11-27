using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Services;
using LearnWithMentorDTO;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWithMentor.Tests.BLL
{
    [TestFixture]
    public class CommentServiceTests
    {
        private CommentService commentService;
        private Mock<IUnitOfWork> uowMock;
        
        [SetUp]
        public void SetUp()
        {
            uowMock = new Mock<IUnitOfWork>();
            commentService = new CommentService(uowMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            commentService.Dispose();
        }

        [Test]
        public async Task GetCommentById_ShouldReturnCommentDTO()
        {
            int commentId = 3;
            string text = "test";
            string fullName = "Full Name";

            uowMock
                .Setup(u => u.Comments.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Comment()
                {
                    Id = commentId,
                    Create_Date = DateTime.Now,
                    Create_Id = 0,
                    Creator = null,
                    Mod_Date = DateTime.Now,
                    PlanTask_Id = 0,
                    PlanTask = null,
                    Text = text
                });
            uowMock
                .Setup(u => u.Users.ExtractFullNameAsync(It.IsAny<int>()))
                .ReturnsAsync(fullName);

            var result = await commentService.GetCommentAsync(commentId);

            Assert.IsInstanceOf(typeof(CommentDTO), result);
            Assert.AreEqual(commentId, result.Id);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(fullName, result.CreatorFullName);
        }
        [Test]
        public async Task AddCommentToPlanTask_ShouldReturnTrue()
        {
            int planTaskId = 3;
            int creatorID = 1;
            string creatorFullName = "Full Name";

            var comment = new CommentDTO(3, "test text", creatorID, creatorFullName, DateTime.Now, null);
            var planTask = new PlanTask { Id = planTaskId };
            var creator = new User { Id = creatorID };

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(creator);
            uowMock
                .Setup(u => u.Comments.AddAsync(It.IsAny<Comment>()))
                .Callback<Comment>(c => planTask.Comments.Add(new Comment
                {
                    Id = c.Id
                }));

            var result = await commentService.AddCommentToPlanTaskAsync(planTaskId, comment);

            Assert.IsTrue(result);
        }
        [Test]
        public async Task AddCommentToPlanTask_ShouldReturnFalseBecauseNoPlanTask()
        {
            int planTaskId = 3;
            int creatorID = 1;
            string creatorFullName = "Full Name";

            var comment = new CommentDTO(3, "test text", creatorID, creatorFullName, DateTime.Now, null);
            PlanTask planTask = null;
            var creator = new User { Id = creatorID };

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(creator);
            uowMock
                .Setup(u => u.Comments.AddAsync(It.IsAny<Comment>()))
                .Callback<Comment>(c => planTask.Comments.Add(new Comment
                {
                    Id = c.Id
                }));

            var result = await commentService.AddCommentToPlanTaskAsync(planTaskId, comment);

            Assert.IsFalse(result);
        }
        [Test]
        public async Task AddCommentToPlanTask_ShouldReturnFalseBecauseNoCreator()
        {
            int planTaskId = 3;
            int creatorID = 1;
            string creatorFullName = "Full Name";

            var comment = new CommentDTO(3, "test text", creatorID, creatorFullName, DateTime.Now, null);
            var planTask = new PlanTask { Id = planTaskId };
            User creator = null;

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(creator);
            uowMock
                .Setup(u => u.Comments.AddAsync(It.IsAny<Comment>()))
                .Callback<Comment>(c => planTask.Comments.Add(new Comment
                {
                    Id = c.Id
                }));

            var result = await commentService.AddCommentToPlanTaskAsync(planTaskId, comment);

            Assert.IsFalse(result);
        }
        [Test]
        public async Task UpdateCommentIdText_ShouldReturnUpdatedCommentText()
        {
            int commentId = 3;
            string newText = "Hello";
            var comment = new Comment()
            {
                Id = commentId,
                Text = "Old Text"
            };

            uowMock
                .Setup(u => u.Comments.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(comment);
            uowMock
                .Setup(u => u.Comments.UpdateAsync(It.IsAny<Comment>()))
                .Callback<Comment>((c) => c.Text = newText);

            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            Assert.AreEqual(newText, comment.Text);
        }
        [Test]
        public async Task UpdateCommentIdText_ShouldReturnFalseBecauseEmptyNewString()
        {
            int commentId = 3;
            string newText = "";
            var comment = new Comment()
            {
                Id = commentId,
                Text = "Old Text"
            };

            uowMock
                .Setup(u => u.Comments.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(comment);
            uowMock
                .Setup(u => u.Comments.UpdateAsync(It.IsAny<Comment>()))
                .Callback<Comment>((c) => c.Text = newText);

            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            Assert.IsFalse(result);
        }
        [Test]
        public async Task UpdateCommentIdText_ShouldReturnFalseBecauseCommentDoesNotExits()
        {
            int commentId = 3;
            string newText = "";
            var comment = new Comment()
            {
                Id = commentId,
                Text = "Old Text"
            };

            uowMock
                .Setup(u => u.Comments.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Comment)null);
            uowMock
                .Setup(u => u.Comments.UpdateAsync(It.IsAny<Comment>()))
                .Callback<Comment>((c) => c.Text = newText);

            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            Assert.IsFalse(result);
        }
        [Test]
        public async Task GetCommentsForPlanTask_ShouldReturnComments()
        {
            int planTaskId = 3;
            var planTask = new PlanTask
            {
                Id = planTaskId,
                Comments = new Comment[] 
                {
                    new Comment { Id = 0, Text = "Comment 1" },
                    new Comment { Id = 1, Text = "Comment 2" }
                }
            };

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.ExtractFullNameAsync(It.IsAny<int>()))
                .ReturnsAsync("Creator Full Name");

            var result = await commentService.GetCommentsForPlanTaskAsync(planTaskId);

            Assert.AreEqual(2, result.ToList().Count);
            for (int i = 0; i < planTask.Comments.Count; i++)
            { 
                Assert.AreEqual(planTask.Comments.ToList()[i].Id, result.ToList()[i].Id);
                Assert.AreEqual(planTask.Comments.ToList()[i].Text, result.ToList()[i].Text);
            }
        }
        [Test]
        public async Task GetCommentsForPlanTask_ShouldReturnNullBecausePlanTaskIsNull()
        {
            int planTaskId = 3;
            PlanTask planTask = null;

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.ExtractFullNameAsync(It.IsAny<int>()))
                .ReturnsAsync("Creator Full Name");

            var result = await commentService.GetCommentsForPlanTaskAsync(planTaskId);

            Assert.IsNull(result);
        }
        [Test]
        public async Task GetCommentsForPlanTask_ShouldReturnEmptyCollectionBecauseNoComments()
        {
            int planTaskId = 3;
            var planTask = new PlanTask
            {
                Id = planTaskId
            };

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.ExtractFullNameAsync(It.IsAny<int>()))
                .ReturnsAsync("Creator Full Name");

            var result = await commentService.GetCommentsForPlanTaskAsync(planTaskId);

            Assert.AreEqual(0, result.ToList().Count);
        }
        [Test]
        public async Task RemoveById_ShouldNotRemoveBecauseDoesNotExist()
        {
            uowMock
                .Setup(u => u.Comments.ContainsIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            int commentId = 3;

            var result = await commentService.RemoveByIdAsync(commentId);

            Assert.IsFalse(result);
        }
    }
}
