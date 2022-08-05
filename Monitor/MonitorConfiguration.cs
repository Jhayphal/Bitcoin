using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Monitor
{
  public sealed class MonitorConfiguration
  {
    [JsonProperty("interval_in_minutes", Required = Required.Always)]
    public int IntervalInMinutes { get; set; }

    [JsonProperty("tolerance", Required = Required.Always)]
    public double Tolerance { get; set; }

    [JsonProperty("cryptocompare_key", Required = Required.Always)]
    public string CryptocompareKey { get; set; }

    [JsonProperty("mail_sender", Required = Required.Always)]
    public MailSender MailSender { get; set; }

    [JsonProperty("mail_receivers", Required = Required.Always)]
    public string[] MailReceivers { get; set; }

    [JsonProperty("checkpoints", Required = Required.Always)]
    public Checkpoint[] Checkpoints { get; set; }

    public string ToJson()
      => JsonConvert.SerializeObject(this, Converter.Settings);

    public void Write(string fileName)
    {
      using (var writer = new StreamWriter(fileName))
      {
        var plainText = ToJson();

        writer.Write(plainText);
      }
    }

    public static MonitorConfiguration FromJson(string json)
      => JsonConvert.DeserializeObject<MonitorConfiguration>(json, Converter.Settings);

    public static MonitorConfiguration Read(string fileName)
    {
      using (var reader = new StreamReader(fileName))
      {
        var plainText = reader.ReadToEnd();

        return FromJson(plainText);
      }
    }
  }

  public sealed class Checkpoint
  {
    [JsonProperty("currency", Required = Required.Always)]
    public string Currency { get; set; }

    [JsonProperty("value", Required = Required.Always)]
    public double Value { get; set; }

    [JsonProperty("higher", Required = Required.Always)]
    public bool Higher { get; set; }

    [JsonProperty("was_sended", Required = Required.Always)]
    public bool WasSended { get; set; }
  }

  public sealed class MailSender
  {
    [JsonProperty("user", Required = Required.Always)]
    public string User { get; set; }

    [JsonProperty("password", Required = Required.Always)]
    public string Password { get; set; }

    [JsonProperty("host", Required = Required.Always)]
    public string Host { get; set; }

    [JsonProperty("port", Required = Required.Always)]
    public int Port { get; set; }

    [JsonProperty("use_ssl", Required = Required.Always)]
    public bool UseSsl { get; set; }

    [JsonProperty("email", Required = Required.Always)]
    public string Email { get; set; }

    [JsonProperty("display_name", Required = Required.AllowNull)]
    public string DisplayName { get; set; }
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
