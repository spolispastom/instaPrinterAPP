﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPatterns.InstagramApi.MediasJsonTypes;

namespace InstagramPatterns.InstagramApi.MediasJsonTypes
{

    public class Caption
    {

        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("from")]
        public From2 From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

}