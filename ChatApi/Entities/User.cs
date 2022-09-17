using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApi.Entities
{
    public class User : IdentityUser<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;

        [NotMapped]
        public string Password { get; set; } = string.Empty;
    
        public List<Message>? SendedMessages { get; set; }
        public List<Message>? ReceivedMessages { get; set; }
        public List<GroupMessage>? GroupMessages { get; set; }
        public List<Group>? Group { get; set; }
    }
}
