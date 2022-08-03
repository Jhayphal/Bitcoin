using Monitor;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace CurrencyMonitor
{
  public partial class MonitorService : ServiceBase
  {
    private const string ConfigFile = @"D:\monitor.conf";
    private readonly CurrencyObserver observer;
    private Timer clock;

    public MonitorService()
    {
      InitializeComponent();

      observer = new CurrencyObserver(new CurrencyRateCache());
      observer.OnFound += (object sender, (double rate, Checkpoint checkpoint) e)
        => SendMails($"Current rate: {e.rate:D2}. Checkpoint: {e.checkpoint.Value:D2}");
    }

    protected override void OnStart(string[] args)
    {
      SetupTimer();
      Run();
    }

    private void Run()
    {
      Observe().GetAwaiter().GetResult();
      
      clock.Start();
    }

    private void SetupTimer()
    {
      var configuration = ReadConfig();
      clock = new Timer(configuration.IntervalInMinutes * 60d * 1000d);
      clock.Elapsed += async (s, e) => await Observe();
    }

    private async Task Observe() => await observer.Observe(ReadConfig());

    protected override void OnStop() => clock.Stop();

    private MonitorConfiguration ReadConfig()
    {
      using (var reader = new StreamReader(ConfigFile))
      {
        var plainText = reader.ReadToEnd();
        
        return MonitorConfiguration.FromJson(plainText);
      }
    }

    private void SendMails(string text)
    {
      MonitorConfiguration configuration = ReadConfig();

      MailAgent agent = new MailAgent(configuration.MailSender);

      foreach (var receiver in configuration.MailReceivers)
        agent.SendMail(receiver, "Checkpoint triggered", text, isHtml: false);
    }
  }
}
