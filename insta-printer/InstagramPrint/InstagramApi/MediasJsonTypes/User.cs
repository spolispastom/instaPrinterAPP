﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPrint.InstagramApi.MediasJsonTypes;

namespace InstagramPrint.InstagramApi.MediasJsonTypes
{

    internal class User
    {

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("profile_picture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }

}
