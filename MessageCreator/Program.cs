using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MessageCreator
{
    class Program
    {
        static bool ValidateConfig(SmtpClient smtpClient)
        {
            if (smtpClient.DeliveryMethod != SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                Console.WriteLine("MessageCreator only works with specificed pickup directory.");
                return false;
            }

            if(string.IsNullOrEmpty(smtpClient.PickupDirectoryLocation))
            {
                Console.WriteLine("No pickup directory location specified.");
                return false;
            }

            return true;
        }

        static void CreateMail(SmtpClient smtpClient)
        {
            var message = new MailMessage()
            {
                Subject = Guid.NewGuid().ToString(),
                Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis sodales est, vitae finibus lacus. Sed elementum at arcu vel consectetur. Vestibulum egestas purus a metus luctus fermentum. Aliquam vestibulum sapien at justo egestas scelerisque. Suspendisse potenti. Mauris hendrerit, tellus eu efficitur pretium, lectus odio mollis turpis, vel interdum libero leo in ligula. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis suscipit vehicula posuere. Proin dapibus sed tortor non ornare. Suspendisse finibus leo ex, et dictum urna elementum et. Vestibulum aliquam, quam condimentum interdum volutpat, justo sem vulputate justo, finibus sollicitudin massa mi congue mauris. Quisque eget scelerisque elit.\n" +
                "Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec nibh tellus, tincidunt vel posuere vel, tempus nec est. Sed vitae metus consectetur, pretium turpis facilisis, pellentesque leo. Vestibulum rutrum purus id odio tempus, vitae hendrerit justo efficitur. Vestibulum efficitur ut sem id auctor. Donec imperdiet purus ut diam vestibulum, suscipit facilisis sapien fringilla. Phasellus consectetur tincidunt velit non mollis. Vivamus porta vulputate dignissim. Suspendisse non vulputate sapien. Duis pharetra rutrum ipsum luctus accumsan. Phasellus gravida quam ut cursus dapibus. Quisque imperdiet vehicula tortor sed porta."
            };

            message.To.Add("recipient@example.com");

            smtpClient.Send(message);
        }

        static void Main(string[] args)
        {
            using(var smtpClient = new SmtpClient())
            {
                if(!ValidateConfig(smtpClient))
                {
                    return;
                }

                ConsoleKeyInfo key;
                do
                {
                    Console.WriteLine("Press space to create a mail in {0}, q to quit.",
                        smtpClient.PickupDirectoryLocation);

                    key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        CreateMail(smtpClient);
                    }

                } while (key.Key != ConsoleKey.Q);
            }
        }
    }
}
