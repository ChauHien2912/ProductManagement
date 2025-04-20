using SE160244.ProductManagement.Repo.Models;

namespace SE160244.ProductManagement.API.Contract.request
{
    public class CreateProductRequest
    {
        public string? ProductName { get; set; }
        public int? UnitsinStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? CategoryId { get; set; }

        
    }
}
