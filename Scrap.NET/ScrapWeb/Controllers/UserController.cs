using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ScrapWeb.DTO;
using ScrapWeb.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScrapWeb.Controllers
{
    [Authorize(Roles = "ROLE_ADMIN")]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private UserService userService;

        public UserController()
        {
            userService = new UserService();
        }

        // GET api/user
        public IEnumerable<UserInfoDTO> Get()
        {
            return userService.getAll();
        }

        // GET api/user/5
        public UserInfoDTO Get(String id)
        {
            return userService.getById(id);
        }

        // POST api/user
        [Authorize(Roles = "ROLE_ADMIN")]
        public UserInfoDTO Post(DTO.UserRegisterDTO user)
        {
            return userService.registerUser(user);
        }

    }
}
