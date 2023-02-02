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
        public ChatReducedDTO(Guid id, User user)
        {
            Id = id;
            Contact = new UserDTO(user);
        }

        public Guid Id { get; set; }
        public UserDTO? Contact { get; set; }
    }
}
