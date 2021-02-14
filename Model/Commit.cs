using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDevOpsHelperLib.Model
{
    public class Commit
    {
        [JsonProperty("CommitId")]
        public string commitId { get; set; }

        [JsonProperty("Url")]
        public string url { get; set; }

        [JsonProperty("workItems")]
        public List<WorkItem> WorkItems { get; set; }
    }
}
