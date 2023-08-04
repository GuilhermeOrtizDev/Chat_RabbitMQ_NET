namespace Chat.Web.Models
{
    public class MessageInputModel
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public MessageInputModel()
        {
            Content = string.Empty;
            CreatedAt = DateTime.Now;
        }
    }
}
