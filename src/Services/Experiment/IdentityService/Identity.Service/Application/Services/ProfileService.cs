using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Service.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// 获取描述数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            context.IssuedClaims = claims.ToList();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 判断用户是否激活
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
