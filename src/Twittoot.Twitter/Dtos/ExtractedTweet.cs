namespace Twittoot.Twitter.Dtos
{
    public class ExtractedTweet
    {
        public long Id { get; set; }
        public string MessageContent { get; set; }
        public string[] MediaUrls { get; set; }
    }
}