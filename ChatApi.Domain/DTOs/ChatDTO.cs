using ChatApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChatApi.Domain.DTOs
{
    public class ChatReducedDTO
    {
        public ChatReducedDTO(Guid id, User user,  ChatMessage? lastMessage, int unseenCount = 0)
        {
            Id = id;
            Contact = new UserDTO(user);
            UnseenCount = unseenCount;
            LastMessage = lastMessage;
        }

        public int UnseenCount { get; set; }
        public Guid Id { get; set; }
        public UserDTO? Contact { get; set; }
        public ChatMessage? LastMessage { get; set; }
    }
}
