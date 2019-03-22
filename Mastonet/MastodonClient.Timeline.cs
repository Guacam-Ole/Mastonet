﻿using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mastonet
{
    partial class MastodonClient
    {
        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetHomeTimeline(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetHomeTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions options)
        {
            string url = "/api/v1/timelines/home";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetList<Status>(url);
        }

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetPublicTimeline(long? maxId = null, long? sinceId = null, int? limit = null, bool local = false)
        {
            return GetPublicTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, local);
        }

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions options, bool local = false)
        {
            string url = "/api/v1/timelines/public";

            var queryParams = "";
            if (local)
            {
                queryParams += "?local=true";
            }
            if (options != null)
            {
                if (queryParams != "")
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }
                queryParams += options.ToQueryString();
            }

            return GetList<Status>(url + queryParams);
        }

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetTagTimeline(string hashtag, long? maxId = null, long? sinceId = null, int? limit = null, bool local = false)
        {
            return GetTagTimeline(hashtag, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, local);
        }

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false)
        {
            string url = "/api/v1/timelines/tag/" + hashtag;

            var queryParams = "";
            if (local)
            {
                queryParams += "?local=true";
            }
            if (options != null)
            {
                if (queryParams != "")
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }
                queryParams += options.ToQueryString();
            }

            return GetList<Status>(url + queryParams);
        }
               
        #region Streaming
        
        public TimelineStreaming GetPublicStreaming()
        {
            return new TimelineStreaming(Instance, "/api/v1/streaming/public", AuthToken.AccessToken);
        }

        public TimelineStreaming GetUserStreaming()
        {
            return new TimelineStreaming(Instance, "/api/v1/streaming/user", AuthToken.AccessToken);
        }

        public TimelineStreaming GetHashtagStreaming(string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", "hashtag");
            }

            return new TimelineStreaming(Instance, "/api/v1/streaming/hashtag?tag=" + hashtag, AuthToken.AccessToken);
        }

        public TimelineStreaming GetDirectMessagesStreaming()
        {
            return new TimelineStreaming(Instance, "/api/v1/streaming/direct", AuthToken.AccessToken);
        }


        [Obsolete("The url is not used, please use GetPublicStreaming() method")]
        public TimelineStreaming GetPublicStreaming(string streamingApiUrl) => GetPublicStreaming();


        [Obsolete("The url is not used, please use GetUserStreaming() method")]
        public TimelineStreaming GetUserStreaming(string streamingApiUrl) => GetUserStreaming();


        [Obsolete("The url is not used, please use GetHashtagStreaming(string hashtag) method")]
        public TimelineStreaming GetHashtagStreaming(string streamingApiUrl, string hashtag) => GetHashtagStreaming(hashtag);

        #endregion
    }
}
