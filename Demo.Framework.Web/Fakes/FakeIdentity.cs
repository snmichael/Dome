using System;
using System.Security.Principal;

namespace Demo.Framework.Web.Fakes
{
    public class FakeIdentity : IIdentity
    {
        private readonly string _name;

        public FakeIdentity(string userName)
        {
            _name = userName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { return !String.IsNullOrEmpty(_name); }
        }

        public string Name
        {
            get { return _name; }
        }

    }
}