namespace IssueTracker.Dal.Models;
public class ChatEntity
{
    public int Id { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
    public String? Info { get; set; }
    public bool IsPersonal { get; set; } = true;
}