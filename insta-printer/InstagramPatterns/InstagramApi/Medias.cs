
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPatterns.InstagramApi.MediasJsonTypes;

namespace InstagramPatterns.InstagramApi
{

    public class Medias
    {

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

}
