namespace Monitor.Tests
{
  internal class TestCurrencyRateProvider : ICurrencyRateProvider
  {
    private readonly double[] samples;
    private int current;

    public TestCurrencyRateProvider(double[] samples)
    {
      this.samples = samples ?? throw new ArgumentNullException(nameof(samples));

      if (samples.Length == 0)
        throw new ArgumentException("Cannot be empty", nameof(samples));
    }

    public Task<double?> GetRate(string currency)
    {
      if (current == samples.Length)
        current = 0;

      return Task.Run<double?>(() => samples[current++]);
    }

    public void Reset()
    {
      // ignore
    }
  }
}
