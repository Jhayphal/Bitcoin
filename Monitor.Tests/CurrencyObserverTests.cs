namespace Monitor.Tests
{
  public class CurrencyObserverTests
  {
    private CurrencyObserver observer;

    [SetUp]
    public void Setup()
    {
      observer = new CurrencyObserver(
        new TestCurrencyRateProvider(
          new double[] { 17000d }
          )
        );
    }

    [Test]
    public async Task FoundNotHigherTest()
    {
      bool found = false;
      observer.OnFound += (s, e) => found = true;

      await observer.Observe(new Checkpoint[]
      {
        new Checkpoint
        {
          Currency = "USD",
          Value = 18000d,
          Higher = false
        }
      });

      Assert.IsTrue(found);
    }

    [Test]
    public async Task FoundHigherTest()
    {
      bool found = false;
      observer.OnFound += (s, e) => found = true;

      await observer.Observe(new Checkpoint[]
      {
        new Checkpoint
        {
          Currency = "USD",
          Value = 16999d,
          Higher = true
        }
      });

      Assert.IsTrue(found);
    }

    [Test]
    public async Task NotFoundHigherTest()
    {
      bool found = false;
      observer.OnFound += (s, e) => found = true;

      await observer.Observe(new Checkpoint[]
      {
        new Checkpoint
        {
          Currency = "USD",
          Value = 17001d,
          Higher = true
        }
      });

      Assert.IsFalse(found);
    }

    [Test]
    public async Task NotFoundNotHigherTest()
    {
      bool found = false;
      observer.OnFound += (s, e) => found = true;

      await observer.Observe(new Checkpoint[]
      {
        new Checkpoint
        {
          Currency = "USD",
          Value = 15699d,
          Higher = false
        }
      });

      Assert.IsFalse(found);
    }
  }
}
