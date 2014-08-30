using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Microsoft.AspNet.Identity;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements IAbpSession to get session informations from ASP.NET Identity framework.
    /// </summary>
    public class AbpSession : IAbpSession, ISingletonDependency
    {
        public long? UserId
        {
            get
            {
                var userId = Thread.CurrentPrincipal.Identity.GetUserId();
                if (userId == null)
                {
                    return null;
                }

                return Convert.ToInt64(userId);
            }
        }

        public int? TenantId
        {
            get
            {
                //enabled de�ilse default'un id'sini al, d�n
                //enabled ise principal'dan al...
                if (!IsMultiTenancyEnabled)
                {
                    return GetDefaultTenantId();
                }

                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return null;
                }

                var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
                if (claim == null)
                {
                    return null;
                }

                return Convert.ToInt32(claim.Value);
            }
        }

        private static int? _defaultTenantId;

        private static int GetDefaultTenantId()
        {
            if (!_defaultTenantId.HasValue)
            {
                using (var tenantRepository = IocHelper.ResolveAsDisposable<IAbpTenantRepository>())
                {
                    var defaultTenant = tenantRepository.Object.FirstOrDefault(t => t.TenancyName == "default");
                    if (defaultTenant == null)
                    {
                        throw new AbpException("A tenant must be exist with TenancyName = 'default'");
                    }

                    _defaultTenantId = defaultTenant.Id;
                }
            }

            return _defaultTenantId.Value;
        }

        private static bool IsMultiTenancyEnabled
        {
            get { return string.Equals(ConfigurationManager.AppSettings["Abp.MultiTenancy.IsEnabled"], "true", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}