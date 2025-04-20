namespace SE160244.ProductManagement.API.Contract.request
{
    public class QueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "";
        public string Filter { get; set; } = "";
        public string IncludeProperties { get; set; } = "";
        public SortOrder SortOrder { get; set; } 
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

}

