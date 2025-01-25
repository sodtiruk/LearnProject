using LearnProject.Dtos.request;
using LearnProject.Models;
using LearnProject.Repositories;
using LearnProject.Utils;

namespace LearnProject.Services
{
    public class ProductService
    {

        private readonly IProductRepository _iProductRepository;

        public ProductService(IProductRepository iProductRepository)
        {
            _iProductRepository = iProductRepository;
        }

        public Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            return _iProductRepository.GetAllProducts();
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var product = await _iProductRepository.GetProductById(id);
            return product ?? throw new KeyNotFoundException($"Product with id {id} not found");
        }

        public async Task<ProductModel> CreateProduct(ProductRequest productRequest)
        {
            
            if (productRequest.Price == null || productRequest.Name == null || productRequest.Price == null)
            {
                throw new KeyNotFoundException($"Product required all argrument");
            }

            return await _iProductRepository.CreateProduct(productRequest);
        }

        public async Task DeleteProductById(int id)
        {
            await _iProductRepository.DeleteProduct(id);
        }

        public async Task<ProductModel> UpdateProductById(int id, ProductRequest productRequest)
        {
            return await _iProductRepository.UpdateProductById(id, productRequest);
        }
    }
}

