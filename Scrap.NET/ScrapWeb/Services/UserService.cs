using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ScrapWeb.DataAccess;
using ScrapWeb.DTO;
using ScrapWeb.Exceptions;
using ScrapWeb.Repositories;
using ScrapWeb.Entities;
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
        protected LogsRepository Logs;
        private ScrapContext scrapContext;

        public UserService()
            : this(Startup.UserManagerFactory(), Startup.OAuthOptions.AccessTokenFormat)
        {
            scrapContext = new ScrapContext();
            Logs = new LogsRepository(scrapContext);
        }

        public UserService(UserManager<IdentityUser> userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            scrapContext = new ScrapContext();
            Logs = new LogsRepository(scrapContext);
        }

        public UserManager<IdentityUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public UserInfoDTO registerUser(UserRegisterDTO model)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = model.username
            };

            IdentityResult result = UserManager.Create(user, model.password);
            if (!result.Succeeded)
            {
                Logs.create(LogEntity.ERROR, "Failed to create a new user: " + result.Errors);
                throw new ServiceException(result.Errors);
            }

            Logs.create(LogEntity.INFO, "Created a new user with username " + user.UserName);
            //scrapContext.SaveChanges();
            return new UserInfoDTO(user);
        }

        public IEnumerable<UserInfoDTO> getAll()
        {
            var dbcontext = new ScrapContext();
            var users = dbcontext.Users.ToList();
            var userinfos = new List<UserInfoDTO>();
            foreach(var user in users) {
                userinfos.Add(new UserInfoDTO(user));
            }
            return userinfos;
        }

        public UserInfoDTO getById(string id)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                throw new ServiceException("User " + id + " was not found", System.Net.HttpStatusCode.NotFound);
            return new UserInfoDTO(user);
        }
    }
}