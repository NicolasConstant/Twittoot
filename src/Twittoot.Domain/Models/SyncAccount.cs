namespace Twittoot.Domain.Models
{
    public class SyncAccount
    {
        public string TwitterName { get; set; }
        public string MastodonName { get; set; }
        public string MastodonInstance { get; set; }
        public string MastodonToken { get; set; }
        public long LastSyncTweetId { get; set; }
    }
}