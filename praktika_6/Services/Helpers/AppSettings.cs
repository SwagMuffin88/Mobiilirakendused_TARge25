using System.Text.Json.Serialization;

namespace praktika_6.Services;

public class AppSettings
{
    [JsonPropertyName("UsdApiKey")]
    public string UsdApiKey { get; set; }
}