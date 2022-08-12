using System.Diagnostics;
using System.ServiceProcess;

namespace CurrencyMonitor
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
      Trace.Listeners.Add(new EventLogTraceListener(nameof(CurrencyMonitor)));

      ServiceBase.Run(new MonitorService());
    }
  }
}
