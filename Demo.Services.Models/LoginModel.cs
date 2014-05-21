using Demo.Framework.Core.DataAnnotations;

namespace Demo.Services.Models
{
    public class LoginModel
    {
        [ResourceDisplayName("Login.UserName")]
        //[Required(ErrorMessageResourceType = typeof (Language), ErrorMessageResourceName = "LoginModel_UserName_Login_UserName")]
        public string UserName { get; set; }

        [ResourceDisplayName("Login.PassWord")]
        public string PassWord { get; set; }

        [ResourceDisplayName("Login.RememberMe")]
        public bool RememberMe { get; set; }
    }
}
