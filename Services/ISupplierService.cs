using ProductSample.Models.ViewModel;

namespace ProductSample.Services
{
    public interface ISupplierService
    {
        Task<List<SuppliersViewModel>> GetAllSupplierAsync();
    }
}
