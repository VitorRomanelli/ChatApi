namespace ChatApi.Application.Models.InputModels
{
    public class UserInputModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string? Pic { get; set; } = string.Empty;
        public string? PicExtension { get; set; } = string.Empty;
    }
}
