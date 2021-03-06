﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPatterns.InstagramApi.MediasJsonTypes;

namespace InstagramPatterns.InstagramApi.MediasJsonTypes
{

    public class Datum
    {

        [JsonProperty("attribution")]
        public object Attribution { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        [JsonProperty("filter")]
        public string Filter { get; set; }

        [JsonProperty("created_time"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("likes")]
        public Likes Likes { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("users_in_photo")]
        public UsersInPhoto[] UsersInPhoto { get; set; }

        [JsonProperty("caption")]
        public Caption Caption { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public User2 User { get; set; }
    }

}
