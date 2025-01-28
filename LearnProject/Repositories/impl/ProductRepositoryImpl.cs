using LearnProject.Data;
using LearnProject.Dtos.request;
using LearnProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Repositories.impl
{
    public class ProductRepositoryImpl : IProductRepository
    {

        private readonly AppDbContext _context;

        public ProductRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductModel?> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<ProductModel> CreateProduct(ProductRequest productRequest)
        {
            if (productRequest.Price == null)
            {
                throw new ArgumentNullException(nameof(productRequest), "Price cannot be null");
            }

            var product = new ProductModel
            {
                Name = productRequest.Name ?? "N/A",
                Description = productRequest.Description ?? "N/A",
                Price = (decimal)productRequest.Price
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id) ??
                throw new KeyNotFoundException($"Product with id {id} not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductModel> UpdateProductById(int id, ProductRequest productRequest)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id) ??
                throw new KeyNotFoundException($"Product with id {id} not found");

            product.Name = productRequest.Name ?? product.Name;
            product.Description = productRequest.Description ?? product.Description;
            product.Price = productRequest.Price ?? product.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
