using System;
using System.Collections.Generic;

namespace SE160244.ProductManagement.Repo.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? UnitsinStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
