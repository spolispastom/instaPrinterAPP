
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstagramPrint.InstagramApi.MediasJsonTypes;

namespace InstagramPrint.InstagramApi
{

    internal class Medias
    {

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

}
