﻿using System;

namespace Twittoot.Domain.Sync.Models
{
    public class SyncAccount
    {
        public Guid Id { get; set; }
        public string TwitterName { get; set; }
        public string MastodonName { get; set; }
        public string MastodonInstance { get; set; }
        public string MastodonAccessToken { get; set; }
        public long LastSyncTweetId { get; set; }
    }
}