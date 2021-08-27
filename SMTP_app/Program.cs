using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace SMTP_app
{
    class Program
    {
        static void Main(string[] args)
        {
            // Данные об отправителе и получателе
            Console.WriteLine("Input your email adress");
            System.String adr = Console.ReadLine();
           
            Console.WriteLine("Input reciver adress");
            System.String tomail = Console.ReadLine();

            Console.WriteLine("Input your password");
            System.String psw = Console.ReadLine();

            Console.WriteLine("Input filepath");
            System.String @fl = Console.ReadLine();

            Console.WriteLine("Input mail subject");
            System.String topic = Console.ReadLine();

            Console.WriteLine("Input mail Body");
            System.String textbody = Console.ReadLine();


            // отправка письма
            try
            {
                MailAddress fromadr = new MailAddress(adr);
                MailAddress toadr = new MailAddress(tomail);

                using (MailMessage mail = new MailMessage(fromadr, toadr))
                using (SmtpClient smtpServer = new SmtpClient())
                {
                    mail.Subject = topic;
                    mail.Body = textbody;
                    System.Net.Mail.Attachment att;
                    att = new Attachment(@fl);
                    mail.Attachments.Add(att);
                    smtpServer.Host = "smtp.gmail.com";
                    smtpServer.Port = 587;
                    smtpServer.EnableSsl = true;
                    smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpServer.UseDefaultCredentials = false;
                    smtpServer.Credentials = new System.Net.NetworkCredential(fromadr.Address, psw);
                    smtpServer.Send(mail);
                    Console.WriteLine("Message sent!");
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Failure!");

            }
            
           
            Console.ReadLine();
        }
    }
}
