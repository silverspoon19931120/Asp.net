﻿using Abp.Zero.NHibernate.EntityMappings;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;

namespace Abp.Zero.SampleApp.NHibernate.Mappings
{
    public class AppTenantMap : TenantMap<Tenant, User>
    {

    }
}
