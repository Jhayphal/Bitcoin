using Monitor;

namespace Bitcoin
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var service = new Service();
      service.OnStart();

      while (true)
      {
        Thread.Sleep(1000);
      }
    }
  }
}