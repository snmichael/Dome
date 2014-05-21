using Demo.Framework.Core.Globalization;
using Demo.Framework.Core.Mvc;

namespace Demo.Framework.Core.DataAnnotations
{
    public class ResourceDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;
        private object[] _args={};
        //private bool _resourceValueRetrived;

        public ResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }
        public ResourceDisplayName(string resourceKey, object[] args)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
            Args = args;

        }

        public string ResourceKey { get; set; }

        public object[] Args { get; set; }

        public override string DisplayName
        {
            get
            {
                _resourceValue = LanuagePack.GetGlobalResource(ResourceKey);
                return _resourceValue;
            }
        }

        public string Name
        {
            get { return "ResourceDisplayName"; }
        }
    }
}
