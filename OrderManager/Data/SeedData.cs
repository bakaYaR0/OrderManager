using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OrderManager.Models;
using System;
using System.IO;
using System.Linq;

namespace OrderManager.Data
{
    public static class SeedData
    {
        public static void SeedProviders(IServiceProvider serviceProvider)
        {
            using (var context = new OrderContext(
                serviceProvider.GetRequiredService<DbContextOptions<OrderContext>>()))
            {
                if (context.Providers.Any())
                {
                    return;
                }

                var providersJson = File.ReadAllText(@".\\Data\\TestData.json");
                Provider[] providers = JsonConvert.DeserializeObject<Provider[]>(providersJson);

                context.Providers.AddRange(providers);
                context.SaveChanges();
            }
        }
    }
}
