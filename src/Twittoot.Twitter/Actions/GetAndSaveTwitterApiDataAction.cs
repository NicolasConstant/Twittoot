﻿using System;
using Twittoot.Twitter.Setup.Settings;
using Twittoot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup.Actions
{
    public class GetAndSaveTwitterApiDataAction
    {
        private readonly ITwitterDevSettingsRepository _twitterDevSettingsRepository;

        #region Ctor
        public GetAndSaveTwitterApiDataAction(ITwitterDevSettingsRepository twitterDevSettingsRepository)
        {
            _twitterDevSettingsRepository = twitterDevSettingsRepository;
        }
        #endregion

        public void Execute()
        {
            Console.WriteLine();
            Console.WriteLine("Provide Twitter API Consumer Key");
            var consumerKey = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Provide Twitter API Consumer Secret");
            var consumerSecret = Console.ReadLine();

            var settings = new TwitterDevApiSettings
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };

            _twitterDevSettingsRepository.SaveTwitterDevApiSettings(settings);
        }
    }
}