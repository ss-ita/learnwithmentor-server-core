using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;
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
            //arrange
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

            //act
            var result = await commentService.GetCommentAsync(commentId);

            //assert
            Assert.IsInstanceOf(typeof(CommentDto), result);
            Assert.AreEqual(commentId, result.Id);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(fullName, result.CreatorFullName);
        }
        [Test]
        public async Task AddCommentToPlanTask_ShouldReturnTrue()
        {
            //arrange
            int planTaskId = 3;
            int creatorID = 1;
            string creatorFullName = "Full Name";

            var comment = new CommentDto(3, "test text", creatorID, creatorFullName, DateTime.Now, null);
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

            //act
            var result = await commentService.AddCommentToPlanTaskAsync(planTaskId, comment);

            //assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task AddCommentToPlanTask_ShouldReturnFalseBecauseNoPlanTask()
        {
            //arrange
            int planTaskId = 3;
            int creatorID = 1;
            string creatorFullName = "Full Name";

            var comment = new CommentDto(3, "test text", creatorID, creatorFullName, DateTime.Now, null);
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

            //act
            var result = await commentService.AddCommentToPlanTaskAsync(planTaskId, comment);

            //assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task AddCommentToPlanTask_ShouldReturnFalseBecauseNoCreator()
        {
            //arrange
            int planTaskId = 3;
            int creatorID = 1;
            string creatorFullName = "Full Name";

            var comment = new CommentDto(3, "test text", creatorID, creatorFullName, DateTime.Now, null);
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

            //act
            var result = await commentService.AddCommentToPlanTaskAsync(planTaskId, comment);

            //assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task UpdateCommentIdText_ShouldReturnUpdatedCommentText()
        {
            //arrange
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

            //act
            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            //assert
            Assert.AreEqual(newText, comment.Text);
        }
        [Test]
        public async Task UpdateCommentIdText_ShouldReturnFalseBecauseEmptyNewString()
        {
            //arrange
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

            //act
            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            //assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task UpdateCommentIdText_ShouldReturnFalseBecauseCommentDoesNotExits()
        {
            //arrange
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

            //act
            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            //assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task GetCommentsForPlanTask_ShouldReturnComments()
        {
            //arrange
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

            //act
            var result = await commentService.GetCommentsForPlanTaskAsync(planTaskId);

            //assert
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
            //arrange
            int planTaskId = 3;
            PlanTask planTask = null;

            uowMock
                .Setup(u => u.PlanTasks.Get(It.IsAny<int>()))
                .ReturnsAsync(planTask);
            uowMock
                .Setup(u => u.Users.ExtractFullNameAsync(It.IsAny<int>()))
                .ReturnsAsync("Creator Full Name");

            //act
            var result = await commentService.GetCommentsForPlanTaskAsync(planTaskId);

            //assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetCommentsForPlanTask_ShouldReturnEmptyCollectionBecauseNoComments()
        {
            //arrange
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

            //act
            var result = await commentService.GetCommentsForPlanTaskAsync(planTaskId);

            //assert
            Assert.AreEqual(0, result.ToList().Count);
        }
        [Test]
        public async Task RemoveById_ShouldNotRemoveBecauseDoesNotExist()
        {
            //arrange
            uowMock
                .Setup(u => u.Comments.ContainsIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            int commentId = 3;

            //act
            var result = await commentService.RemoveByIdAsync(commentId);

            //assert
            Assert.IsFalse(result);
        }
    }
}
