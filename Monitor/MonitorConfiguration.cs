using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Monitor
{
  public partial class MonitorConfiguration
  {
    [JsonProperty("interval_in_minutes", Required = Required.Always)]
    public long IntervalInMinutes { get; set; }

    [JsonProperty("mail_receivers", Required = Required.Always)]
    public string[] MailReceivers { get; set; }

    [JsonProperty("mail_sender", Required = Required.Always)]
    public MailSender MailSender { get; set; }

    [JsonProperty("cryptocompare_key", Required = Required.Always)]
    public string CryptocompareKey { get; set; }

    [JsonProperty("checkpoints", Required = Required.Always)]
    public Checkpoint[] Checkpoints { get; set; }
  }

  public partial class Checkpoint
  {
    [JsonProperty("currency", Required = Required.Always)]
    public string Currency { get; set; }

    [JsonProperty("value", Required = Required.Always)]
    public double Value { get; set; }

    [JsonProperty("higher", Required = Required.Always)]
    public bool Higher { get; set; }
  }

  public partial class MailSender
  {
    [JsonProperty("user", Required = Required.Always)]
    public string User { get; set; }

    [JsonProperty("password", Required = Required.Always)]
    public string Password { get; set; }

    [JsonProperty("host", Required = Required.Always)]
    public string Host { get; set; }

    [JsonProperty("email", Required = Required.Always)]
    public string Email { get; set; }

    [JsonProperty("display_name", Required = Required.Always)]
    public string DisplayName { get; set; }
  }

  public partial class MonitorConfiguration
  {
    public static MonitorConfiguration FromJson(string json) => JsonConvert.DeserializeObject<MonitorConfiguration>(json, Converter.Settings);
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
