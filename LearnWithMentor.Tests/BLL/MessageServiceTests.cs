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
    class MessageServiceTests
    {
        private MessageService messageService;
        private Mock<IUnitOfWork> uowMock;

        [SetUp]
        public void SetUp()
        {
            uowMock = new Mock<IUnitOfWork>();
            messageService = new MessageService(uowMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            messageService.Dispose();
        }

        [Test]
        public async Task GetTaskAsync_ShouldReturnMessageDTOs()
        {
            //arrange
            int userTaskIdTest = 1;
            string fullNameTest = "Full Name Test";
            var userTask = new UserTask()
            {
                Id = userTaskIdTest,
                Messages = new Message[]
                {
                    new Message{Id = 0,Text = "Test 0"},
                    new Message{Id = 1,Text = "Test 1"}
                }
            };

            uowMock
                 .Setup(u => u.UserTasks.GetAsync(It.IsAny<int>()))
                 .ReturnsAsync(userTask);
            uowMock
                .Setup(u => u.Users.ExtractFullNameAsync(It.IsAny<int>()))
                .ReturnsAsync(fullNameTest);
            //act 
            var result = await messageService.GetMessagesAsync(userTaskIdTest);

            //asert
            Assert.AreEqual(2, result.ToList().Count);
            for (int i = 0; i < userTask.Messages.Count; i++)
            {
                Assert.AreEqual(userTask.Messages.ToList()[i].Id, result.ToList()[i].Id);
                Assert.AreEqual(userTask.Messages.ToList()[i].Text, result.ToList()[i].Text);
            }
        }

        [Test]
        public async Task UpdateIsReadStateAsync_ShouldReturnTrue()
        {
            //arrange
            int userTaskIdTest = 1;
            var messageDtoTest = new MessageDto(1, 2, 3, "Test", "TestText", DateTime.Now, true);
            var messageTest = new Message() { Id = messageDtoTest.Id };

            uowMock
                .Setup(u => u.Messages.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(messageTest);
            uowMock
                .Setup(u => u.Save());

            //act
            var result = await messageService.UpdateIsReadStateAsync(userTaskIdTest, messageDtoTest);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SendMessage_ShouldReturnTrue()
        {
            //arrange
            var newMessageTest = new MessageDto(1, 2, 3, "Test", "TestText", DateTime.Now, true);
            var userTaskTest = new UserTask();

            uowMock
               .Setup(u => u.Messages.AddAsync(It.IsAny<Message>()))
               .Callback<Message>(c => userTaskTest.Messages.Add(new Message
               {
                   User_Id = newMessageTest.Id,
                   Text = newMessageTest.Text,
                   UserTask_Id = newMessageTest.UserTaskId
               }));
            uowMock
                .Setup(u => u.Save());
            //act 
            var result = messageService.SendMessage(newMessageTest);

            //assert
            Assert.IsTrue(result);
        }
    }
}
