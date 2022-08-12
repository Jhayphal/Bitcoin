using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Monitor
{
  public sealed class Service
  {
    private const string ConfigFile = @"D:\monitor.conf";

    private readonly CurrencyObserver observer = new CurrencyObserver(new CurrencyRateCache());
    private readonly Timer clock = new Timer();

    private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

    public bool Running { get; private set; }

    public void OnStart()
    {
      observer.OnFound += (object sender, (double rate, Checkpoint checkpoint) e)
        => SendMails($"Current rate: {e.rate:D2}. Checkpoint: {e.checkpoint.Value:D2}");

      clock.AutoReset = false;
      clock.Elapsed += async (s, e) => await Inspect(tokenSource.Token);

      Task.Run(() => Inspect(tokenSource.Token));
    }

    public void OnStop()
    {
      clock.Dispose();

      while (Running)
      {
        Thread.Sleep(50);
      }
    }

    private async Task Inspect(CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
      {
        OnStop();

        return;
      }

      Running = true;

      MonitorConfiguration configuration;

      try
      {
        configuration = MonitorConfiguration.Read(ConfigFile);

        await observer.Observe(configuration);

        configuration.Write(ConfigFile);
      }
      catch (Exception e)
      {
        Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);

        return;
      }
      finally
      {
        Running = false;
      }

      if (!tokenSource.IsCancellationRequested)
      {
        clock.Interval = configuration.IntervalInMinutes * 60d * 1000d;
        clock.Start();
      }
    }

    private void SendMails(string text)
    {
      MonitorConfiguration configuration = MonitorConfiguration.Read(ConfigFile);

      MailAgent agent = new MailAgent(configuration.MailSender);

      foreach (var receiver in configuration.MailReceivers)
      {
        try
        {
          agent.SendMail(receiver, "Checkpoint triggered", text, isHtml: false);
        }
        catch (Exception e)
        {
          Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
        }
      }
    }
  }
}
