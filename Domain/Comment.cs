using System;

namespace Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public virtual AppUser Author { get; set; } //virtual cause we are using lazy loading
        public virtual Activity Activity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}