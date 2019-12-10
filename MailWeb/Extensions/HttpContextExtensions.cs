using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MailWeb.Extensions
{
    public static class HttpContextExtensions
    {
        #region Methods

        public static Guid GetTenantId(this HttpContext httpContext)
        {
            var initialSiteId = httpContext.Request
                .Headers.Where(header => header.Key == "X-Site-Id")
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            if (!Guid.TryParse(initialSiteId, out var siteId))
                return Guid.Empty;

            return siteId;
        }

        #endregion
    }
}