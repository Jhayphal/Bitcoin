using Monitor;
using System.ServiceProcess;

namespace CurrencyMonitor
{
  public partial class MonitorService : ServiceBase
  {
    private readonly Service service = new Service();

    public MonitorService()
    {
      InitializeComponent();
    }

    protected override void OnStart(string[] args) => service.OnStart();

    protected override void OnStop() => service.OnStop();
  }
}
