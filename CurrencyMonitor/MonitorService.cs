using Monitor;
using System.ServiceProcess;

namespace CurrencyMonitor
{
  public partial class MonitorService : ServiceBase
  {
    public const string Name = nameof(CurrencyMonitor);

    private readonly Service service = new Service();

    public MonitorService()
    {
      InitializeComponent();

      AutoLog = false;
      ServiceName = Name;
      CanPauseAndContinue = false;
      CanHandlePowerEvent = false;
      CanHandleSessionChangeEvent = false;
      CanShutdown = false;
      CanStop = true;
    }

    protected override void OnStart(string[] args) => service.OnStart();

    protected override void OnStop() => service.OnStop();
  }
}
