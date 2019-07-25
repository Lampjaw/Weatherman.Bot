using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Weatherman.Data.Models;

namespace Weatherman.Data
{
    public class WeathermanDbContext : DbContext
    {
        public WeathermanDbContext(DbContextOptions<WeathermanDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ServerProfile> ServerProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyConfigurations(builder);
            ConvertToConvention(builder);
        }

        private static void ApplyConfigurations(ModelBuilder builder)
        {
            var applyGenericMethods = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            var applyGenericApplyConfigurationMethods = applyGenericMethods.Where(a => a.IsGenericMethod && a.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));
            var applyGenericMethod = applyGenericApplyConfigurationMethods.FirstOrDefault(a => a.GetParameters().FirstOrDefault()?.ParameterType.Name == "IEntityTypeConfiguration`1");

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(builder, new[] { Activator.CreateInstance(type) });
                        break;
                    }
                }
            }
        }

        private static void ConvertToConvention(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = ToSnakeCase(entity.Relational().TableName);

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = ToSnakeCase(property.Name);
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = $"p_k_{ToSnakeCase(key.Relational().Name)}";
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = $"f_k_{ToSnakeCase(key.Relational().Name)}";
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = $"i_x_{ToSnakeCase(index.Relational().Name)}";
                }
            }
        }

        private static string ToSnakeCase(string input)
        {
            var matches = Regex.Matches(input, "([A-Z][A-Z0-9]*(?=$|[A-Z][a-z0-9])|[A-Za-z][a-z0-9]+)");
            var result = string.Join("_", matches.Select(a => a.Value));

            return result.ToLower();
        }
    }
}
