﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using BugTracker.Models;
using System.Net.Mail;
using Mailjet.Client;
using Mailjet.Client.Resources;
using System.Net;
using System.Text;
using System.Net.Configuration;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Mailjet.Client.TransactionalEmails;

namespace BugTracker
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Following with MailJet API Documentation https://github.com/mailjet/mailjet-apiv3-dotnet for a simple email
            MailjetClient client = new MailjetClient("fbd5291bd68ff822ba83c14ef2e1911b", "5eae6a21c963041ddbf15bd8c3bc2a5b");

            MailjetRequest request = new MailjetRequest // Not needed for a simple send email. Used for Get/Put/Delete/Post api calls
            {
                Resource = Send.Resource,
            };
            
            // Email Message
            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("noreply@buggyboy.dev"))
                .WithSubject(message.Subject)
                .WithHtmlPart(message.Body)
                .WithTo(new SendContact(message.Destination))
                .Build();

            await client.SendTransactionalEmailAsync(email);

            

            // Plug in your email service here to send an email.
            //SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            //using(MailMessage mailMessage = new MailMessage())
            //{
            //    mailMessage.From = new MailAddress(smtpSection.From);
            //    mailMessage.To.Add(message.Destination);
            //    mailMessage.Subject = message.Subject;
            //    mailMessage.IsBodyHtml = true;
            //    mailMessage.Body = message.Body;

            //    SmtpClient smtp = new SmtpClient();
            //    smtp.DeliveryMethod = smtpSection.DeliveryMethod;
            //    smtp.Host = smtpSection.Network.Host;
            //    smtp.EnableSsl = smtpSection.Network.EnableSsl;
            //    smtp.UseDefaultCredentials = false;
            //    NetworkCredential networkCredential = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
            //    smtp.Credentials = networkCredential;
            //    smtp.Port = smtpSection.Network.Port;
            //    try
            //    {
            //        await smtp.SendMailAsync(mailMessage);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //return Task.FromResult(0);
        }

    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
