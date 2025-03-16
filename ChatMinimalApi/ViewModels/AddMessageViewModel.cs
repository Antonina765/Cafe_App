namespace ChatMinimalApi.ViewModels
{
    public class AddMessageViewModel
    {
        public string Text { get; set; }
        
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
