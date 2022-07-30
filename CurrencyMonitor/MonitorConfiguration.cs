using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CurrencyMonitor
{
  public class MonitorConfiguration
  {
    [JsonProperty("interval_in_minutes", Required = Required.Always)]
    public long IntervalInMinutes { get; set; }

    [JsonProperty("emails", Required = Required.Always)]
    public string[] Emails { get; set; }

    [JsonProperty("cryptocompare_key", Required = Required.Always)]
    public string CryptocompareKey { get; set; }

    [JsonProperty("checkpoints", Required = Required.Always)]
    public Checkpoint[] Checkpoints { get; set; }

    public static MonitorConfiguration FromJson(string json) => JsonConvert.DeserializeObject<MonitorConfiguration>(json, Converter.Settings);
  }

  public class Checkpoint
  {
    [JsonProperty("currency", Required = Required.Always)]
    public string Currency { get; set; }

    [JsonProperty("value", Required = Required.Always)]
    public double Value { get; set; }

    [JsonProperty("higher", Required = Required.Always)]
    public bool Higher { get; set; }
  }

  public static class Serialize
  {
    public static string ToJson(this MonitorConfiguration self) => JsonConvert.SerializeObject(self, Converter.Settings);
  }

  internal static class Converter
  {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
      DateParseHandling = DateParseHandling.None,
      Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
  }
}
