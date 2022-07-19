using ProductSample.Models.ViewModel;

namespace ProductSample.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllCategoryAsync();

    }
}
