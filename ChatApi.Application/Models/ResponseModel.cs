namespace ChatApi.Application.Models
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Content { get; set; }

        public ResponseModel(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public ResponseModel(int status, object content)
        {
            Status = status;
            Content = content;
        }

        public ResponseModel(int status, string message, object content)
        {
            Status = status;
            Message = message;
            Content = content;
        }
    }
}
