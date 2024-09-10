using Microsoft.AspNetCore.Identity;

namespace IssueTracker.Domain.Models;

public class Chat
{
    public int Id { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
    public String? Info { get; set; }
    public bool IsPersonal { get; set; } = true;
}