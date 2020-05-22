using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBoard.Models;
using MessageBoard.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace MessageBoard.Test
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task CanAddValidMessage()
        {
            // Arrange
            var messageRepository = Mock.Of<IMessageRepository>();
            var messageService = new MessageService(messageRepository, NullLogger<MessageService>.Instance);

            // Act
            var result = await messageService.Add(new MessageRequest
            {
                AddedBy = "TestUser",
                Contents = "Hello"
            });

            // Assert
            Assert.Equal(AddMessageResult.Ok, result);
        }

        [Fact]
        public async Task InvalidMessageReturnsValidationError()
        {
            // Arrange
            var messageRepository = Mock.Of<IMessageRepository>();
            var messageService = new MessageService(messageRepository, NullLogger<MessageService>.Instance);

            // Act
            var result = await messageService.Add(new MessageRequest
            {
                AddedBy = "TestUser",
                Contents = null,
            });

            // Assert
            Assert.Equal(AddMessageResult.MissingProperties, result);
        }

        [Fact]
        public async Task GetAllReturnsResultsFromMessageRepository()
        {
            // Arrange
            var expectedUser = "testuser";
            var expectedContents = "testcontents";
            var expectedDate = DateTime.UtcNow;
            var testResults = new List<Message>
            {
                new Message { AddedBy = expectedUser, AddedDate = expectedDate, Contents = expectedContents }
            };
            var messageRepository = new Mock<IMessageRepository>();
            messageRepository.Setup(r => r.GetAll())
                .ReturnsAsync((IReadOnlyList<Message>)testResults);

            var messageService = new MessageService(messageRepository.Object, NullLogger<MessageService>.Instance);

            // Act
            var results = await messageService.GetAll();

            // Assert
            Assert.NotNull(results);
            var single = Assert.Single(results);
            Assert.Equal(expectedUser, single.AddedBy);
            Assert.Equal(expectedContents, single.Contents);
            Assert.Equal(expectedDate, single.AddedDate);
        }

        [Fact]
        public async Task MessageRepositoryErrorCaught()
        {
            // Arrange
            var messageRepository = new Mock<IMessageRepository>();
            messageRepository.Setup(r => r.GetAll())
                .ThrowsAsync(new InvalidOperationException("Oh no"));

            var messageService = new MessageService(messageRepository.Object, NullLogger<MessageService>.Instance);

            // Act
            var results = await messageService.GetAll();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }
    }
}
