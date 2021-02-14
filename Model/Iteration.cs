using Newtonsoft.Json;

namespace AzureDevOpsHelperLib.Model
{
    public class Iteration
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
