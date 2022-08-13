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

  public class MonitorConfiguration : Changeable
  {
    protected int intervalInMinutes;
    protected double tolerance;
    protected string cryptocompareKey;
    protected MailSender mailSender;
    protected string[] mailReceivers;
    protected Checkpoint[] checkpoints;

    public override bool HasChanges => base.HasChanges || MailSender.HasChanges || Checkpoints.Any(c => c.HasChanges);

    [JsonProperty("interval_in_minutes", Required = Required.Always)]
    public virtual int IntervalInMinutes
    {
      get => intervalInMinutes;
      set => SetValue(ref intervalInMinutes, value);
    }

    [JsonProperty("tolerance", Required = Required.Always)]
    public virtual double Tolerance
    {
      get => tolerance;
      set => SetValue(ref tolerance, value);
    }

    [JsonProperty("cryptocompare_key", Required = Required.Always)]
    public virtual string CryptocompareKey
    {
      get => cryptocompareKey;
      set => SetValue(ref cryptocompareKey, value);
    }

    [JsonProperty("mail_sender", Required = Required.Always)]
    public virtual MailSender MailSender
    {
      get => mailSender;
      set => SetValue(ref mailSender, value);
    }

    [JsonProperty("mail_receivers", Required = Required.Always)]
    public virtual string[] MailReceivers
    {
      get => mailReceivers;
      set => SetValue(ref mailReceivers, value);
    }

    [JsonProperty("checkpoints", Required = Required.Always)]
    public virtual Checkpoint[] Checkpoints
    {
      get => checkpoints;
      set => SetValue(ref checkpoints, value);
    }

    public string ToJson()
      => JsonConvert.SerializeObject(this, Converter.Settings);

    public virtual void Write(string fileName)
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

  public class Checkpoint : Changeable
  {
    protected string currency;
    protected double value;
    protected bool higher;
    protected bool wasSended;

    [JsonProperty("currency", Required = Required.Always)]
    public virtual string Currency
    {
      get => currency;
      set => SetValue(ref currency, value);
    }

    [JsonProperty("value", Required = Required.Always)]
    public virtual double Value
    {
      get => value;
      set => SetValue(ref this.value, value);
    }

    [JsonProperty("higher", Required = Required.Always)]
    public virtual bool Higher
    {
      get => higher;
      set => SetValue(ref higher, value);
    }

    [JsonProperty("was_sended", Required = Required.Always)]
    public virtual bool WasSended
    {
      get => wasSended;
      set => SetValue(ref wasSended, value);
    }
  }

  public class MailSender : Changeable
  {
    protected string user;
    protected string password;
    protected string host;
    protected int port;
    protected bool useSsl;
    protected string email;
    protected string displayName;

    [JsonProperty("user", Required = Required.Always)]
    public virtual string User
    {
      get => user;
      set => SetValue(ref user, value);
    }

    [JsonProperty("password", Required = Required.Always)]
    public virtual string Password
    {
      get => password;
      set => SetValue(ref password, value);
    }

    [JsonProperty("host", Required = Required.Always)]
    public virtual string Host
    {
      get => host;
      set => SetValue(ref host, value);
    }

    [JsonProperty("port", Required = Required.Always)]
    public virtual int Port
    {
      get => port;
      set => SetValue(ref port, value);
    }

    [JsonProperty("use_ssl", Required = Required.Always)]
    public virtual bool UseSsl
    {
      get => useSsl;
      set => SetValue(ref useSsl, value);
    }

    [JsonProperty("email", Required = Required.Always)]
    public virtual string Email
    {
      get => email;
      set => SetValue(ref email, value);
    }

    [JsonProperty("display_name", Required = Required.AllowNull)]
    public virtual string DisplayName
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
