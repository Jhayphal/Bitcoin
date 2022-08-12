using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Monitor
{
  public abstract class Changeable
  {
    [JsonIgnore]
    public virtual bool HasChanges { get; private set; }

    protected void SetValue<T>(ref T param, T value)
    {
      param = value;

      HasChanges = true;
    }
  }

  public sealed class MonitorConfiguration : Changeable
  {
    private int intervalInMinutes;
    private double tolerance;
    private string cryptocompareKey;
    private MailSender mailSender;
    private string[] mailReceivers;
    private Checkpoint[] checkpoints;

    public override bool HasChanges => base.HasChanges || MailSender.HasChanges || Checkpoints.Any(c => c.HasChanges);

    [JsonProperty("interval_in_minutes", Required = Required.Always)]
    public int IntervalInMinutes
    {
      get => intervalInMinutes;
      set => SetValue(ref intervalInMinutes, value);
    }

    [JsonProperty("tolerance", Required = Required.Always)]
    public double Tolerance
    {
      get => tolerance;
      set => SetValue(ref tolerance, value);
    }

    [JsonProperty("cryptocompare_key", Required = Required.Always)]
    public string CryptocompareKey
    {
      get => cryptocompareKey;
      set => SetValue(ref cryptocompareKey, value);
    }

    [JsonProperty("mail_sender", Required = Required.Always)]
    public MailSender MailSender
    {
      get => mailSender;
      set => SetValue(ref mailSender, value);
    }

    [JsonProperty("mail_receivers", Required = Required.Always)]
    public string[] MailReceivers
    {
      get => mailReceivers;
      set => SetValue(ref mailReceivers, value);
    }

    [JsonProperty("checkpoints", Required = Required.Always)]
    public Checkpoint[] Checkpoints
    {
      get => checkpoints;
      set => SetValue(ref checkpoints, value);
    }

    public string ToJson()
      => JsonConvert.SerializeObject(this, Converter.Settings);

    public void Write(string fileName)
    {
      if (HasChanges)
      {
        File.WriteAllText(fileName, ToJson());
      }
    }

    public static MonitorConfiguration FromJson(string json)
      => JsonConvert.DeserializeObject<MonitorConfiguration>(json, Converter.Settings);

    public static MonitorConfiguration Read(string fileName) => FromJson(File.ReadAllText(fileName));
  }

  public sealed class Checkpoint : Changeable
  {
    private string currency;
    private double value;
    private bool higher;
    private bool wasSended;

    [JsonProperty("currency", Required = Required.Always)]
    public string Currency
    {
      get => currency;
      set => SetValue(ref currency, value);
    }

    [JsonProperty("value", Required = Required.Always)]
    public double Value
    {
      get => value;
      set => SetValue(ref this.value, value);
    }

    [JsonProperty("higher", Required = Required.Always)]
    public bool Higher
    {
      get => higher;
      set => SetValue(ref higher, value);
    }

    [JsonProperty("was_sended", Required = Required.Always)]
    public bool WasSended
    {
      get => wasSended;
      set => SetValue(ref wasSended, value);
    }
  }

  public sealed class MailSender : Changeable
  {
    private string user;
    private string password;
    private string host;
    private int port;
    private bool useSsl;
    private string email;
    private string displayName;

    [JsonProperty("user", Required = Required.Always)]
    public string User
    {
      get => user;
      set => SetValue(ref user, value);
    }

    [JsonProperty("password", Required = Required.Always)]
    public string Password
    {
      get => password;
      set => SetValue(ref password, value);
    }

    [JsonProperty("host", Required = Required.Always)]
    public string Host
    {
      get => host;
      set => SetValue(ref host, value);
    }

    [JsonProperty("port", Required = Required.Always)]
    public int Port
    {
      get => port;
      set => SetValue(ref port, value);
    }

    [JsonProperty("use_ssl", Required = Required.Always)]
    public bool UseSsl
    {
      get => useSsl;
      set => SetValue(ref useSsl, value);
    }

    [JsonProperty("email", Required = Required.Always)]
    public string Email
    {
      get => email;
      set => SetValue(ref email, value);
    }

    [JsonProperty("display_name", Required = Required.AllowNull)]
    public string DisplayName
    {
      get => displayName;
      set => SetValue(ref displayName, value);
    }
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
