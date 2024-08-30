using Serilog;
using System.Data;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace WebMagazine.Api.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        //public ICollection<ArticleTag> ArticlesTags { get; set; }
    }
}
