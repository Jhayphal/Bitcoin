using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BitcoininfoProvider
{
  public class BitcoinExchangeRates
  {
    [JsonProperty("15m", Required = Required.Always)]
    public double The15M { get; set; }

    [JsonProperty("last", Required = Required.Always)]
    public double Last { get; set; }

    [JsonProperty("buy", Required = Required.Always)]
    public double Buy { get; set; }

    [JsonProperty("sell", Required = Required.Always)]
    public double Sell { get; set; }

    [JsonProperty("symbol", Required = Required.Always)]
    public string Symbol { get; set; } = string.Empty;

    public static Dictionary<string, BitcoinExchangeRates>? FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, BitcoinExchangeRates>>(json, Converter.Settings);
  }

  public static class Serialize
  {
    public static string ToJson(this Dictionary<string, BitcoinExchangeRates> self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
