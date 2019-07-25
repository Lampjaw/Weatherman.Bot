using static Weatherman.Data.DbContextHelper;

namespace Weatherman.Data
{
    public interface IDbContextHelper
    {
        DbContextFactory GetFactory();
    }
}