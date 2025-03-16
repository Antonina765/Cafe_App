namespace ChatMinimalApi.ViewModels
{
    public class ChatMessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
