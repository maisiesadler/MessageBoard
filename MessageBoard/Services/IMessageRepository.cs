using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBoard.Models;

namespace MessageBoard.Services
{
    public interface IMessageRepository
    {
        Task<IReadOnlyList<Message>> GetAll();
        
        Task<bool> Add(Message message);
    }
}
