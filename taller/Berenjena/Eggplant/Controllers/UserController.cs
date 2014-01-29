using Eggplant.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Eggplant.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private UserService userService;

        public UserController()
        {
            userService = new UserService();
        }

        // GET api/user
        public object Get()
        {
            return null;// userService.getAll();
        }

        // GET api/user/5
        public object Get(int id)
        {
            return null;// userService.getById(id);
        }

        // POST api/user
        public object Post(JObject value)
        {
            return null;// userService.registerUser(user);
        }
    }
}
