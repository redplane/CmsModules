using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace MailWeb.Services.Implementations
{
    public class SiteCorsPolicyProvider : ICorsPolicyProvider
    {
        #region Properties

        public virtual async Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
