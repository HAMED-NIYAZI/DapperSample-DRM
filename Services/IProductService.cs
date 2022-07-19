using ProductSample.Models.ViewModel;

namespace ProductSample.Services
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetAllAsync();
        Task<ProductViewModel> GetByIdAsync(int id);
        Task<ProductResult> AddAsync(ProductViewModel productViewModel);
        Task<ProductResult> BulkAddAsync(List<ProductViewModel> productViewModel);
        Task<ProductResult> AddWithSPAsync(ProductViewModel productViewModel);
        Task UpdateAsync(ProductViewModel productViewModel);
        Task DeleteByIdAsync(int id);


    }

    public enum ProductResult 
    { 
        SuccessFull,
        Dupplicate,
        Failed
    }
}
