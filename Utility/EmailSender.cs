// Utility/EmailSender.cs

using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace StudentTaskManager.Utility
{
    // Yeh class IEmailSender interface ko implement karti hai lekin koi action nahi karti.
    public class EmailSender : IEmailSender
    {
        // IEmailSender ko sirf is method ki zarurat hoti hai
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Yahan hum koi email nahi bhej rahe hain, bas Task.CompletedTask return kar rahe hain.
            // Agar tum future mein SendGrid ya SMTP use karoge, toh is code ko badalna hoga.
            return Task.CompletedTask;
        }
    }
}