using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Abp.Zero.EntityFramework
{
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class AbpZeroTenantDbContext<TRole, TUser,TSelf> : AbpZeroCommonDbContext<TRole, TUser,TSelf>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf: AbpZeroTenantDbContext<TRole, TUser, TSelf>
    {

        protected AbpZeroTenantDbContext(DbContextOptions<TSelf> options)
            :base(options)
        {

        }
    }
}