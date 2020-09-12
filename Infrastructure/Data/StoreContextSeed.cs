using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var item in brands)
                    {
                        await context.ProductBrands.AddAsync(item).ConfigureAwait(false);
                    }
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        await context.ProductTypes.AddAsync(item).ConfigureAwait(false);
                    }
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }

                
                if (!context.Products.Any())
                {
                    var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    foreach (var item in products)
                    {
                        await context.Products.AddAsync(item).ConfigureAwait(false);
                    }
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}