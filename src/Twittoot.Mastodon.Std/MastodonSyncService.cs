using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using mastodon;
using mastodon.Enums;
using Twittoot.Mastodon.Std.Models;
using Twittoot.Mastodon.Std.Repositories;

namespace Twittoot.Mastodon.Std
{
    public interface IMastodonSyncService
    {
        Task<IEnumerable<AttachementResult>> SubmitAttachementsAsync(string accessToken, string mastodonInstance, string[] attachementUrls);
        Task SubmitTootAsync(string accessToken, string mastodonInstance, string lastTweetFullText, int[] attachementsIds);
    }

    public class MastodonSyncService : IMastodonSyncService
    {
        private readonly Dictionary<string, MastodonClient> _mastodonClientDict;

        #region Ctor
        public MastodonSyncService()
        {
            _mastodonClientDict = new Dictionary<string, MastodonClient>();
        }
        #endregion
        
        public async Task SubmitTootAsync(string accessToken, string mastodonInstance, string lastTweetFullText, int[] attachementsIds)
        {
            var client = GetClient(mastodonInstance);
            await client.PostNewStatusAsync(accessToken, lastTweetFullText, StatusVisibilityEnum.Public, -1, attachementsIds);
        }

        public async Task<IEnumerable<AttachementResult>> SubmitAttachementsAsync(string accessToken, string mastodonInstance, string[] attachementUrls)
        {
            var client = GetClient(mastodonInstance);

            var results = new List<AttachementResult>();
            foreach (var attachementUrl in attachementUrls)
            {
                int? id = null;
                var uploadSucceed = true;
                try
                {
                    byte[] imageBytes;
                    using (var webClient = new WebClient())
                    {
                        imageBytes = webClient.DownloadData(attachementUrl);
                    }

                    var uri = new Uri(attachementUrl);
                    var filename = Path.GetFileName(uri.AbsolutePath);

                    id = (await client.UploadingMediaAttachmentAsync(accessToken, filename, imageBytes, filename)).id; //TODO reintegrate failed URL to toot message
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    uploadSucceed = false;
                }

                var result = new AttachementResult()
                {
                    AttachementId = id ?? -1,
                    UploadSucceeded = uploadSucceed,
                    AttachementUrl = attachementUrl
                };
                results.Add(result);
            }

            return results;
        }

        private MastodonClient GetClient(string mastodonInstanceUrl)
        {
            if (!_mastodonClientDict.ContainsKey(mastodonInstanceUrl))
            {
                var newClient = new MastodonClient(mastodonInstanceUrl);
                _mastodonClientDict.Add(mastodonInstanceUrl, newClient);
            }

            return _mastodonClientDict[mastodonInstanceUrl];
        }
    }
}
