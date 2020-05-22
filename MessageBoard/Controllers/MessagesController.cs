using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBoard.Models;
using MessageBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MessageBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IMessageService messageService,
            ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Message>> Get()
        {
            var messages = await _messageService.GetAll();
            _logger.LogInformation($"Request for messages, returning {messages.Count} messages");
            return messages;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MessageRequest request)
        {
            var addResult = await _messageService.Add(request);

            if (addResult == AddMessageResult.Ok)
            {
                _logger.LogInformation($"Added message by {request.AddedBy}");
                return Ok();
            }
            else if (addResult == AddMessageResult.MissingProperties)
            {
                _logger.LogInformation($"Could not add message by {request.AddedBy} because it was missing properties");
                return BadRequest(new { message = "Missing properties, expected 'AddedBy' and 'Contents'" });
            }
            else
            {
                _logger.LogError($"Could not add message by {request.AddedBy} because of an internal error");
                return StatusCode(500);
            }
        }
    }
}
