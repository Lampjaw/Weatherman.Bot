using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Weatherman.Data
{
    public class DbContextHelper : IDbContextHelper
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbContextHelper(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public DbContextFactory GetFactory()
        {
            return new DbContextFactory(_scopeFactory);
        }

        public class DbContextFactory : IDisposable
        {
            private readonly IServiceScope _scope;
            private readonly WeathermanDbContext _dbContext;

            public DbContextFactory(IServiceScopeFactory scopeFactory)
            {
                _scope = scopeFactory.CreateScope();
                _dbContext = _scope.ServiceProvider.GetRequiredService<WeathermanDbContext>();

                _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            public WeathermanDbContext GetDbContext()
            {
                return _dbContext;
            }

            public void Dispose()
            {
                _dbContext?.Dispose();
                _scope.Dispose();
            }
        }
    }
}
