using Monitor;

namespace Bitcoin
{
  internal class Program
  {
    static void Main()
    {
      var service = new Service();
      service.OnStart();

      while (true)
      {
        Thread.Sleep(50);
      }
    }
  }
}