using System;

using Newtonsoft.Json;

namespace ConductorAPI.Models
{
    [Serializable]
    public class Response
    {
        [JsonProperty("speech")]
        public string Speech { get; set; }

        [JsonProperty("displayText")]
        public string DisplayText { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("contextOut")]
        public string ContextOut { get; set; }

        [JsonProperty("source")]
        public string Source => "ConductorAPI";
    }
}