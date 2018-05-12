using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Common.MessagingService;

namespace UserManagementAPI.Identity.Services
{
  

    public class EmailServiceKW : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var messagingClient = new UserManagementAPI.Common.MessagingService.MessageServiceClient();

            IList<RecipientData> listOfReceipients = new List<RecipientData>();
            listOfReceipients.Add(new RecipientData { EmailAddress = message.Destination });

            await messagingClient.SendEmailAsync(new Common.MessagingService.EmailData
                {
                    IsHtml = true,
                    Body = message.Body,
                    Recipients = listOfReceipients.ToArray(),
                    Subject = message.Subject
                });
        }
    }
}