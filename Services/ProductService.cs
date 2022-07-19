using Dapper;
using ProductSample.Models.ViewModel;
using ProductSample.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace ProductSample.Services
{
    public class ProductService : IProductService
    {
        private readonly DapperUtility _dapperUtility;
        public ProductService(DapperUtility dapperUtility)
        {
            _dapperUtility = dapperUtility;
        }

        public async Task<ProductResult> AddAsync(ProductViewModel productViewModel)
        {
            var sqlQuery = @"INSERT INTO Products (ProductName,SupplierID,CategoryID,UnitPrice)
                            VALUES(@ProductName, @SupplierID, @CategoryID, @UnitPrice)";
            int res = 0;
            using (var db = _dapperUtility.GetMyConnection())
            {
                res = await db.ExecuteAsync(sqlQuery, productViewModel);
            }


            if (res == 0) return ProductResult.Failed;
            return ProductResult.SuccessFull;
        }

        //public async Task<ProductResult> AddWithSPAsync(ProductViewModel productViewModel)
        //{
        //    var sqlQuery = "SP_Products_Insert";
        //    int res = 0;
        //    using (var db = _dapperUtility.GetMyConnection())
        //    {
        //        res = await db.ExecuteAsync(sqlQuery, 
        //            new { ProductName=productViewModel.ProductName, SupplierID=productViewModel.SupplierID,
        //                CategoryID= productViewModel.CategoryID, UnitPrice= productViewModel.UnitPrice}
        //            ,commandType:CommandType.StoredProcedure);

        //    }


        //    if (res == 0) return ProductResult.Failed;
        //    return ProductResult.SuccessFull;
        //}


        public async Task<ProductResult> AddWithSPAsync(ProductViewModel productViewModel)
        {
            var sqlQuery = "SP_Products_Insert";
             var parameters = new DynamicParameters();
            parameters.Add("ProductName", productViewModel.ProductName);
            parameters.Add("SupplierID", productViewModel.SupplierID);
            parameters.Add("CategoryID", productViewModel.CategoryID);
            parameters.Add("UnitPrice", productViewModel.UnitPrice);
            parameters.Add("id",dbType:DbType.Int32,direction:ParameterDirection.Output);


            using (var db = _dapperUtility.GetMyConnection())
            {
                  await db.ExecuteAsync(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
            int id = parameters.Get<int>("id");


            if ( id == 0 ) return ProductResult.Failed;
            return ProductResult.SuccessFull;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE ProductID=@ProductID";
            using (var db = _dapperUtility.GetMyConnection())
            {
                 await db.ExecuteAsync(sql,new { ProductID=id });
            }
        }

        public async Task<List<ProductViewModel>> GetAllAsync()
        {
            var sqlQuery = @"SELECT p.ProductID,p.ProductName,p.SupplierID ,s.CompanyName,
                            p.CategoryID,c.CategoryName,p.UnitPrice
                            FROM Products p
                            left join Suppliers s on p.SupplierID = s.SupplierID
                            left join Categories c on p.CategoryID = c.CategoryID";
            IEnumerable<ProductViewModel> result;
            using (var db = _dapperUtility.GetMyConnection())
            {
                result = await db.QueryAsync<ProductViewModel>(sqlQuery);
            }
            return result.ToList();
        }

        public async Task<ProductViewModel> GetByIdAsync(int id)
        {
            var sql = @"SELECT  ProductID,ProductName,SupplierID,CategoryID,UnitPrice
                          FROM Products 
                          Where ProductID=@ProductID";

            var result = new ProductViewModel(); 

            using (var db=_dapperUtility.GetMyConnection())
            {
                result = await db.QuerySingleOrDefaultAsync<ProductViewModel>(sql,new { ProductID =id});
            }
            return result;
        }

        public async Task UpdateAsync(ProductViewModel productViewModel)
        {
            var sql = @"UPDATE Products
                        SET ProductName = @ProductName, CategoryID = @CategoryID,
                        SupplierID = @SupplierID, UnitPrice = @UnitPrice
                        WHERE ProductID = @ProductID";

            using var db = _dapperUtility.GetMyConnection();
            await db.ExecuteAsync(sql, productViewModel);
        }
    }
}
 