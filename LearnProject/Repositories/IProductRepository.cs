using LearnProject.Dtos.request;
using LearnProject.Models;

namespace LearnProject.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<ProductModel?> GetProductById(int id);
        Task<ProductModel> CreateProduct(ProductRequest productRequest);
        Task<ProductModel> UpdateProductById(int id, ProductRequest productRequest);
        Task DeleteProduct(int id);
    }
}
