namespace Easy.Core.Flow.DbFunction.Module
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
