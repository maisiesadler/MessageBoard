using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBoard.Models;

namespace MessageBoard.Services
{
    public class MessageRepository : IMessageRepository
    {
        private IList<Message> _messages = new List<Message>();
        private object _lock = new object();

        public Task<bool> Add(Message message)
        {
            lock (_lock)
            {
                _messages.Add(message);
            }

            return Task.FromResult(true);
        }

        public Task<IReadOnlyList<Message>> GetAll()
        {
            return Task.FromResult((IReadOnlyList<Message>)_messages);
        }
    }
}
