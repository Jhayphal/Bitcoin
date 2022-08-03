using System.Net;
using System.Net.Mail;
using System.Text;

namespace Monitor
{
  public sealed class MailAgent
  {
    private readonly MailSender sender;

    public MailAgent(MailSender sender)
    {
      this.sender = sender;
    }

    public void SendMail(string receiver, string subject, string text, bool isHtml)
    {
      SmtpClient smtpClient = new SmtpClient(sender.Host)
      {
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(sender.User, sender.Password)
      };

      MailAddress from = new MailAddress(sender.Email, sender.DisplayName);
      MailAddress to = new MailAddress(receiver);

      MailMessage mail = new MailMessage(from, to)
      {
        Subject = subject,
        SubjectEncoding = Encoding.UTF8,

        Body = text,
        BodyEncoding = Encoding.UTF8,

        IsBodyHtml = isHtml
      };

      smtpClient.Send(mail);
    }
  }
}
