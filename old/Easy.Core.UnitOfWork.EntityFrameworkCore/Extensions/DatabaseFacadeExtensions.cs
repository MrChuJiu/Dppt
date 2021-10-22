using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static bool IsRelational(this DatabaseFacade database)
        {
            return database.GetInfrastructure().GetService<IRelationalConnection>() != null;
        }
    }
}
