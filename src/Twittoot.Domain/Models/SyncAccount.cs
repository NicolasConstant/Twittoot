using System;

namespace Twittoot.Domain.Models
{
    public class SyncAccount
    {
        public Guid Id { get; set; }
        public string TwitterName { get; set; }
        public string MastodonName { get; set; }
        public string MastodonInstance { get; set; }
        public string MastodonRefreshToken { get; set; }
        public long LastSyncTweetId { get; set; }
    }
}