using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public class EmailRequest
    {
        private readonly string smtpHost;
        private readonly int? smtpPortNumber;
        private readonly string emailPickupDirectoryLocation;

        public EmailRequest(string smtpHost, int? smtpPortNumber, string emailPickupDirectoryLocation)
        {
            this.smtpHost = smtpHost;
            this.smtpPortNumber = smtpPortNumber;
            this.emailPickupDirectoryLocation = emailPickupDirectoryLocation;
        }

        public Task<string> SendTextEmailAsync(string from, string to, string subject, string body, string ccList = null, string bccList = null)
        {
            return SendEmailAsync(false, from, to, subject, body, ccList, bccList);
        }

        public Task<string> SendHtmlEmailAsync(string from, string to, string subject, string body, string ccList = null, string bccList = null)
        {
            return SendEmailAsync(true, from, to, subject, body, ccList, bccList);
        }

        private async Task<string> SendEmailAsync(bool isBodyHtml, string from, string to, string subject, string body, string ccList = null, string bccList = null)
        {
            try
            {
                if (smtpHost.IsNullOrWhiteSpace())
                {
                    return $"Unable to send the email - SmtpHost was not provided!";
                }

                using var mailMessage = new MailMessage(from, to, subject, body)
                {
                    Subject = subject,
                    IsBodyHtml = isBodyHtml
                };

                // Add CC list.
                if (ccList.HasValue())
                {
                    mailMessage.CC.Add(ccList);
                }

                // Add BCC list.
                if (bccList.HasValue())
                {
                    mailMessage.Bcc.Add(bccList);
                }

                // Setup smtpClient.
                using var smtpClient = new SmtpClient(smtpHost)
                {
                    UseDefaultCredentials = true,
                };

                if (smtpPortNumber.IsPositiveValue())
                {
                    smtpClient.Port = smtpPortNumber.Value;
                }

                if (!string.IsNullOrWhiteSpace(emailPickupDirectoryLocation))
                {
                    if (!Directory.Exists(emailPickupDirectoryLocation))
                    {
                        Directory.CreateDirectory(emailPickupDirectoryLocation);
                    }

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailPickupDirectoryLocation;
                }

                // Send.
                await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
