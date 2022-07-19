using Dapper;
using ProductSample.Models.ViewModel;
using ProductSample.Utilities;

namespace ProductSample.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly DapperUtility _dapperUtility;
        public SupplierService(DapperUtility dapperUtility)
        {
            _dapperUtility = dapperUtility;
        }

        public async Task<List<SuppliersViewModel>> GetAllSupplierAsync()
        {
            var sqlQuery = "select SupplierID,CompanyName  from Suppliers order by CompanyName";
            IEnumerable<SuppliersViewModel> res ;
            using (var db=_dapperUtility.GetMyConnection())
            {
                res = await db.QueryAsync<SuppliersViewModel>(sqlQuery);
            }
            return res.ToList();
        }
    }
}
