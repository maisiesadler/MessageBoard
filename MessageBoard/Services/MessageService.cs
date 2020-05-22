using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MessageBoard.Models;
using System;

namespace MessageBoard.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageService> _logger;

        public MessageService(
            IMessageRepository messageRepository,
            ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<AddMessageResult> Add(MessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AddedBy))
                return AddMessageResult.MissingProperties;

            if (string.IsNullOrWhiteSpace(request.Contents))
                return AddMessageResult.MissingProperties;

            try
            {
                var added = await _messageRepository.Add(new Message
                {
                    AddedBy = request.AddedBy,
                    AddedDate = DateTime.UtcNow,
                    Contents = request.Contents,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception thrown while adding new message");
                return AddMessageResult.Error;
            }

            return AddMessageResult.Ok;
        }

        public async Task<IReadOnlyList<Message>> GetAll()
        {
            try
            {
                var messages = await _messageRepository.GetAll();
                _logger.LogInformation($"Request for messages, returning {messages.Count} messages");
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception thrown getting messages");
            }

            return new List<Message>();
        }
    }
}
