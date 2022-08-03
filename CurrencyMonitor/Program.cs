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
      ServiceBase[] ServicesToRun;
      ServicesToRun = new ServiceBase[]
      {
                new MonitorService()
      };
      ServiceBase.Run(ServicesToRun);
    }
  }
}
