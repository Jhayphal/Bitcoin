using System.Threading.Tasks;

namespace Monitor
{
  public interface ICurrencyRateProvider
  {
    Task<double?> GetRate(string currency);

    void Reset();
  }
}
