using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBoard.Models;

namespace MessageBoard.Services
{
    public interface IMessageService
    {
        Task<IReadOnlyList<Message>> GetAll();
        
        Task<AddMessageResult> Add(MessageRequest request);
    }
}
