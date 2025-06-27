using ChocolateAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace ChocolateAPI.Entites
{
    public class User : IdentityUser
    {
        public UserRole Role { get; set; } = UserRole.User;
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
