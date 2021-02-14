using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDevOpsHelperLib.Model
{
    public class WorkItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
