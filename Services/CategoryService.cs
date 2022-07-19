using Dapper;
using ProductSample.Models.ViewModel;
using ProductSample.Utilities;

namespace ProductSample.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DapperUtility _dapperUtility;
        public CategoryService(DapperUtility dapperUtility)
        {
            _dapperUtility = dapperUtility;
        }


        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var sqlQuery = "select  CategoryID,CategoryName from Categories order by CategoryName";
            IEnumerable<CategoryViewModel> res;
            using (var db = _dapperUtility.GetMyConnection())
            {
                res = await db.QueryAsync<CategoryViewModel>(sqlQuery);
            }
            return res.ToList();
        }
    }
}
