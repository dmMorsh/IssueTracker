namespace IssueTracker.Dal.Models;
public class ChatEntity
{
    public int id { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
    public String? info { get; set; }
    public bool isPersonal { get; set; } = true;
}