using System;

namespace MessageBoard.Models
{
    public class Message
    {
        public string AddedBy { get; set; }
        
        public DateTime AddedDate { get; set; }
        
        public string Contents { get; set; }
    }
}
