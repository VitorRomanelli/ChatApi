using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApi.Domain.Entities
{
    public class User : IdentityUser<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;

        [NotMapped]
        public string Password { get; set; } = string.Empty;
    
        public List<ChatMessage>? SendedMessages { get; set; }
        public List<ChatMessage>? ReceivedMessages { get; set; }
        public List<ChatGroupMessage>? GroupMessages { get; set; }
        public List<ChatGroup>? Group { get; set; }
    }
}
