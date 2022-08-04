namespace Monitor.Tests
{
  public class MailAgentTests
  {
    private MailAgent agent;

    [SetUp]
    public void Setup()
    {
      agent = new MailAgent(new MailSender
      {
        User = "*******@gmail.com",
        Password = "********",
        DisplayName = "Bitcoin mail test",
        Email = "*********@gmail.com",
        Host = "smtp.gmail.com",
        Port = 587,
        UseSsl = true
      });
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