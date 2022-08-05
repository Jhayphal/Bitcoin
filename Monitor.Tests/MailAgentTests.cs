namespace Monitor.Tests
{
  public class MailAgentTests
  {
    private MailAgent agent;

    [SetUp]
    public void Setup()
    {
      var configuration = MonitorConfiguration.Read(@"D:\monitor.conf");

      agent = new MailAgent(configuration.MailSender);
    }

    [Test]
    public void MailTest()
    {
      agent.SendMail(
        receiver: "fen2xxc@yandex.ru",
        subject: "Mail test",
        text: $"{DateTime.Now:F} {nameof(MailTest)}",
        isHtml: false);
    }
  }
}