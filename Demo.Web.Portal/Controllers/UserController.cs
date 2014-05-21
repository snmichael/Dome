using System.Web.Mvc;
using Demo.IService;
using Demo.Services.Models;
using Demo.Web.Utility;

namespace Demo.Web.Portal.Controllers
{
    public class UserController : BaseController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            var list = _userService.Get();
            return View(list);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(UserDto model)
        {
            try
            {
                _userService.Add(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

      
    }
}
