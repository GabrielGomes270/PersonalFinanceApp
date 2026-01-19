namespace PersonalFinanceApp.Shared
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
