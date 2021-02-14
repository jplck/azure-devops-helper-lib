using Newtonsoft.Json;
using System;

namespace AzureDevOpsHelperLib.Model
{
    public class Repository
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("remoteUrl")]
        public Uri RemoteUrl { get; set; }
    }
}
