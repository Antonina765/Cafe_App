namespace ChatMinimalApi.DbStuff.Models
{
    public class Message : BaseModel
    {
        public string Text { get; set; }
        
        public int AuthorId { get; set; }
        
        /// <summary>
        /// This is duplicated data but we need it for perfomance
        /// </summary>
        public string AuthorName { get; set; }
    }
}
