using Demo.Web.Framework.DataAnnotations;

namespace Demo.Services.Models
{
    public class User
    {
        [ResourceDisplayName("User.Id")]
        public long Id { get; set; }

        [ResourceDisplayName("User.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("User.Description")]
        public string Description { get; set; }

        [ResourceDisplayName("User.Job")]
        public Job Job { get; set; }
    }
}