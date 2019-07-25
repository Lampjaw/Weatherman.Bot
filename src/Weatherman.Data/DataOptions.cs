namespace Weatherman.Data
{
    internal class DataOptions
    {
        public string DBConnectionString { get; set; }
        public int PoolSize { get; set; } = 5;
    }
}
