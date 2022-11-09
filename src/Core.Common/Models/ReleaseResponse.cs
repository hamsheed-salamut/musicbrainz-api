using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.Common.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }

        [JsonProperty("sort-name")]
        public string SortName { get; set; }

        [JsonProperty("iso-3166-1-codes")]
        public List<string> Iso31661Codes { get; set; }
    }

    public class Artist
    {
        public string id { get; set; }
        public string name { get; set; }

        [JsonProperty("sort-name")]
        public string SortName { get; set; }
    }

    public class ArtistCredit
    {
        public string name { get; set; }
        public Artist artist { get; set; }
        public string joinphrase { get; set; }
    }

    public class Label
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class LabelInfo
    {
        [JsonProperty("catalog-number")]
        public string CatalogNumber { get; set; }
        public Label label { get; set; }
    }

    public class Medium
    {
        public string format { get; set; }

        [JsonProperty("disc-count")]
        public int DiscCount { get; set; }

        [JsonProperty("track-count")]
        public int TrackCount { get; set; }
    }

    public class Release
    {
        public string id { get; set; }
        public int score { get; set; }

        [JsonProperty("status-id")]
        public string StatusId { get; set; }
        public int count { get; set; }
        public string title { get; set; }
        public string status { get; set; }

        [JsonProperty("text-representation")]
        public TextRepresentation TextRepresentation { get; set; }

        [JsonProperty("artist-credit")]
        public List<ArtistCredit> ArtistCredit { get; set; }

        [JsonProperty("release-group")]
        public ReleaseGroup ReleaseGroup { get; set; }
        public string date { get; set; }
        public string country { get; set; }

        [JsonProperty("release-events")]
        public List<ReleaseEvent> ReleaseEvents { get; set; }
        public string barcode { get; set; }
        public string asin { get; set; }

        [JsonProperty("label-info")]
        public List<LabelInfo> LabelInfo { get; set; }

        [JsonProperty("track-count")]
        public int TrackCount { get; set; }
        public List<Medium> media { get; set; }

        [JsonProperty("packaging-id")]
        public string PackagingId { get; set; }
        public string packaging { get; set; }
    }

    public class ReleaseEvent
    {
        public string date { get; set; }
        public Area area { get; set; }
    }

    public class ReleaseGroup
    {
        public string id { get; set; }

        [JsonProperty("type-id")]
        public string TypeId { get; set; }

        [JsonProperty("primary-type-id")]
        public string PrimaryTypeId { get; set; }
        public string title { get; set; }

        [JsonProperty("primary-type")]
        public string PrimaryType { get; set; }
    }

    public class Root
    {
        public DateTime created { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        public List<Release> releases { get; set; }
    }

    public class TextRepresentation
    {
        public string language { get; set; }
        public string script { get; set; }
    }


}
