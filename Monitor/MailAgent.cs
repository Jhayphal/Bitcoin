using System.Net;
using System.Net.Mail;

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
      MailAddress from = new MailAddress(sender.Email, sender.DisplayName);
      MailAddress to = new MailAddress(receiver);

      SmtpClient smtpClient = new SmtpClient(sender.Host, sender.Port)
      {
        EnableSsl = sender.UseSsl,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(sender.User, sender.Password)
      };

      MailMessage mail = new MailMessage(from, to)
      {
        Subject = subject,
        Body = text,
        IsBodyHtml = isHtml
      };

      smtpClient.Send(mail);
    }
  }
}
