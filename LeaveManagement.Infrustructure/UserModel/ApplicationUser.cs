using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Infrustructure.UserModel
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}
