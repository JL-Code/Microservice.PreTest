using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Service
{
    /// <summary>
    /// 自定义验证密码模式
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            bool isAuthenticated = new TestUserStore().Authenticate(context.UserName, context.Password, out User loginUser);
            if (!isAuthenticated)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid client credential");
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: new Claim[] {
                        new Claim("Name", context.UserName),
                        new Claim("Id", loginUser.Id.ToString()),
                        new Claim("RealName", loginUser.RealName),
                        new Claim("Email", loginUser.Email)
                    }
                );
            }

            return Task.CompletedTask;
        }
    }

    public class TestUserStore
    {

        public List<User> testUsers = new List<User>
        {
            new User{
                Email = "admin@admin.com",
                Password="1",
                UserName="admin"
            },
            new User{
                Email = "jiangy@admin.com",
                Password="1",
                UserName="jiangy"
            }
        };

        public bool Authenticate(string userName, string password, out User testUser)
        {
            testUser = testUsers.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            return testUser != null;
        }
    }

    public class User
    {

        public string UserName { get; set; } = "";

        public string Password { get; set; } = "";

        public string Email { get; set; } = "";

        public string RealName { get; set; } = "";

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
