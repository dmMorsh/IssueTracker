using Microsoft.AspNetCore.Identity;

namespace IssueTracker.Models;

public partial class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<ChatEntity>? Chats { get; set; }

        public ICollection<ApplicationUser>? Friends { get; set; }

        public ICollection<ApplicationUser>? FriendRequests { get; set; }

        public ICollection<ApplicationUser>? Subscriptions { get; set; }

        public ICollection<ExecutionList>? ExecutionList { get; set; }

        public ICollection<WatchList>? WatchList { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }