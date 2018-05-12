using NLog;
using System;
using System.Collections.Generic;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.MessagingService;
using UserManagementAPI.Identity;

namespace UserManagementAPI.BusinessLogic
{
    public class UtilityManager : IUtilityManager
    {
        private readonly IUserManager _userManager;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public UtilityManager(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public GenericActionResult<string> SendEmailToUser(string emailAddress, string subject, string body)
        {
            return _userManager.SendEmailToUserViaEmail(emailAddress, subject, body);
        }

        public GenericActionResult<string> SendEmail(string emailAddress, string subject, string body)
        {
            try
            {
                var messagingService = new MessageServiceClient();

                var receipents = new List<RecipientData>();
                receipents.Add(new RecipientData { EmailAddress = emailAddress, Name = "Pay and Go Team" });

                var emailData = new EmailData()
                {
                    IsHtml = true,
                    Body = body,
                    Subject = subject,
                    Recipients = receipents.ToArray()
                };

                messagingService.SendEmail(emailData);

                return new GenericActionResult<string>() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return new GenericActionResult<string>() { IsSuccess = false, };
            }
        }
    }
}