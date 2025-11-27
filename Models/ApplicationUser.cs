using Microsoft.AspNetCore.Identity;

namespace StudentTaskManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }   
}
