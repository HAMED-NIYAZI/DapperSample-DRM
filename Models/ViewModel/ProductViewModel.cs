namespace ProductSample.Models.ViewModel
{
    public class ProductViewModel
    {

         public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? SupplierID { get; set; }
        public string? CompanyName { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public double? UnitPrice { get; set; }

    } 
}
