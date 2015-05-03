﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPatterns.InstagramApi.MediasJsonTypes;

namespace InstagramPatterns.InstagramApi.MediasJsonTypes
{

    public class Images
    {

        [JsonProperty("low_resolution")]
        public LowResolution LowResolution { get; set; }

        [JsonProperty("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        [JsonProperty("standard_resolution")]
        public StandardResolution StandardResolution { get; set; }
    }

}
