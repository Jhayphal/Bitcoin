using Bitcoininfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyMonitor
{
  internal class CurrencyRateCache
  {
    private readonly Dictionary<string, double?> cache = new Dictionary<string, double?>();

    public async Task<double?> GetRate(string currency)
    {
      if (cache.TryGetValue(currency, out var rate))
        return rate;

      cache[currency] = rate = (await CoinRateInfo.GetActual(currency))?.Last;

      return rate;
    }

    public void Clear() => cache.Clear();
  }
}
