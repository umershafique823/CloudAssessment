using Newtonsoft.Json;

namespace SimeonCloudSoftware.Model
{
    /// <summary>
    /// Group class
    /// </summary>
    public class Group
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }
}
