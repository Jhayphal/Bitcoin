using Bitcoininfo;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Timer = System.Timers.Timer;

namespace CurrencyMonitor
{
  public partial class MonitorService : ServiceBase
  {
    private const string ConfigFile = @"D:\monitor.conf";

    private readonly CurrencyRateCache cache = new CurrencyRateCache();
    
    private MonitorConfiguration configuration;
    private Timer clock;

    public MonitorService()
    {
      InitializeComponent();
    }

    protected override void OnStart(string[] args)
    {
      ReadConfig(ConfigFile);
      SetupTimer();
      Run();
    }

    private void Run()
    {
      Clock_Elapsed(this, null);
      
      clock.Start();
    }

    private void SetupTimer()
    {
      clock = new Timer(configuration.IntervalInMinutes * 60d * 1000d);
      clock.Elapsed += Clock_Elapsed;
    }

    private async void Clock_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        foreach (var checkpoint in configuration.Checkpoints)
        {
          var rate = await cache.GetRate(checkpoint.Currency);

          if (rate.HasValue)
            Check(rate.Value, checkpoint, configuration.Emails);
        }
      }
      finally
      {
        cache.Clear();
      }
    }

    private void Check(double rate, Checkpoint checkpoint, string[] emails)
    {
      if (checkpoint.Higher ? rate > checkpoint.Value : rate < checkpoint.Value)
      {
        SendMails(emails);
      }
    }

    private void SendMails(string[] emails)
    {
      throw new NotImplementedException();
    }

    protected override void OnStop()
    {
      clock.Stop();
    }

    private void ReadConfig(string filePath)
    {
      using (var reader = new StreamReader(filePath))
      {
        var plainText = reader.ReadToEnd();
        configuration = MonitorConfiguration.FromJson(plainText);
      }
    }
  }
}
