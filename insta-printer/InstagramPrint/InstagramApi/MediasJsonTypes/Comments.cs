﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPrint.InstagramApi.MediasJsonTypes;

namespace InstagramPrint.InstagramApi.MediasJsonTypes
{

    internal class Comments
    {

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("data")]
        public Datum2[] Data { get; set; }
    }

}