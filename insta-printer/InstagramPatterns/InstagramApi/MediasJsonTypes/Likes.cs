﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPatterns.InstagramApi.MediasJsonTypes;

namespace InstagramPatterns.InstagramApi.MediasJsonTypes
{

    public class Likes
    {

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("data")]
        public Datum3[] Data { get; set; }
    }

}
