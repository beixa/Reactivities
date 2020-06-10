namespace Domain
{
    public class UserFollowing
    {
        public string ObserverId { get; set; }
        public virtual AppUser Observer { get; set; }//navigation property so we use virtual
        public string TargetId { get; set; }
        public virtual AppUser Target { get; set; } //follower
    }
}