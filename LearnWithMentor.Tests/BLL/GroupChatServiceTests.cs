using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentor.BLL.Services;
using LearnWithMentorDTO;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LearnWithMentor.Tests.BLL
{
	[TestFixture]
	public class GroupChatServiceTests
	{
		private GroupChatService groupChatService;
		private Mock<IUnitOfWork> uowMock;

		[SetUp]
		public void SetUp()
		{
			uowMock = new Mock<IUnitOfWork>();
			groupChatService = new GroupChatService(uowMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			groupChatService.Dispose();
		}

		[Test]
		public async Task GetMessageByGroupId_ShouldReturnGroupChatMessageDTO()
		{
			int messageId = 1;
			int groupId = 1;
			int amount = 1;
			string text = "test";
			var message = new GroupChatMessage()
			{
				Id = 0,
				TextMessage = text,
				UserId = 9,
				GroupId = 1,
				Time = DateTime.Now
			};
			List<GroupChatMessage> messages = new List<GroupChatMessage>();
			messages.Add(message);

			uowMock
				.Setup(u => u.GroupChatMessage.GetGroupMessagesAsync(It.IsAny<int>()))
				.ReturnsAsync(messages);

			var result = await groupChatService.GetGroupMessagesAsync(groupId , amount);

			//Assert.AreEqual(messageId, result.ElementAt(0).MessageId);
			Assert.AreEqual(text, result.First().TextMessage);
		}

	}
}
