using System.Collections.Generic;

namespace Demo.Web.Utility.Globalization.Models
{
    /// <summary>
    /// Language class for installation process
    /// </summary>
    public partial class Language
    {
        public Language()
        {
            Resources = new List<ResourceItem>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDefault { get; set; }
        public bool IsRightToLeft { get; set; }

        public List<ResourceItem> Resources { get; protected set; }
    }
}