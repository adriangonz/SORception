using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ScrapWeb.DTO;
using ScrapWeb.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ScrapWeb.Services
{
    public class UserService
    {

        public UserService()
            : this(Startup.UserManagerFactory(), Startup.OAuthOptions.AccessTokenFormat)
        {
        }

        public UserService(UserManager<IdentityUser> userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public UserManager<IdentityUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public UserInfoDTO registerUser(UserRegisterDTO model)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = model.username
            };

            IdentityResult result = UserManager.Create<IdentityUser>(user, model.password);
            if (!result.Succeeded)
                throw new ServiceException(result.Errors);

            return new UserInfoDTO
            {
                id = user.Id,
                username = user.UserName,
                isAdmin = false
            };
        }
    }
}