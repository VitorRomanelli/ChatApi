namespace ChatApi.Application.Extensions
{
    public static class ListExtensions
    {
        public static PaginatedObject ReturnPaginated<T>(this List<T> items, int? currentPage = 1, int pageSize = 10)
        {
            Pager pager = new Pager(items.Count(), currentPage, pageSize);

            return new PaginatedObject(items.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(), pager);
        }
    }
}
