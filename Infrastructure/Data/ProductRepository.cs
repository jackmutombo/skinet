using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _contect;
        public ProductRepository(StoreContext contect)
        {
            _contect = contect;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _contect.ProductBrands.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _contect.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
           return await _contect.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _contect.ProductTypes.ToListAsync().ConfigureAwait(false);
        }
    }
}