using Bitcoininfo;
using System.Text;

namespace Bitcoin
{
  internal class Program
  {
    static readonly CoinRateManiac maniac = new();

    static void Main(string[] args)
    {
      MainAsync().GetAwaiter().GetResult();

      maniac.Dispose();
    }

    static async Task MainAsync()
    {
      await maniac.Fup();

      Console.ReadLine();
    }
  }

  class CoinRateManiac : IDisposable
  {
    const string JournalFileName = "rate.json";

    readonly StreamWriter streamWriter = new(path: JournalFileName, append: true);
    readonly HttpClient client = new();
    readonly StringBuilder builder = new();

    public void Dispose()
    {
      streamWriter.Dispose();
      client.Dispose();
    }

    public async Task Fup()
    {
      //var jsonResponse = await client.GetStringAsync("https://blockchain.info/ru/ticker"); // https://www.blockchain.com/ru/api/exchange_rates_api

      //var bitcoinExchangeRates = BitcoinExchangeRates.FromJson(jsonResponse) ?? throw new ArgumentNullException();
      var rate = await CoinRateInfo.GetActual("USD");
      Console.WriteLine($"Rate {rate?.Last} at {DateTime.Now:t}");

      //Write(jsonResponse);
    }

    void Write(string rawJson)
    {
      DropSpaces(rawJson);
      streamWriter.WriteLine(builder);
      builder.Clear();
    }

    void DropSpaces(string json)
    {
      foreach (var c in json)
        if (!char.IsWhiteSpace(c))
          builder.Append(c);
    }
  }
}