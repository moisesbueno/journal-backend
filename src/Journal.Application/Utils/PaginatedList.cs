namespace Journal.Application.Utils
{
    public class PaginatedList<T> where T : class
    {
        public PaginatedList(List<T> items, int pageSize, int pageNumber, long total)
        {
            Items = items;
            PageSize = pageSize;
            PageNumber = pageNumber;
            Total = total;
        }

        public List<T> Items { get; }
        public int PageSize { get; }
        public int PageNumber { get; }
        public long Total { get; }

        public long TotalPages =>
            Convert.ToInt64(Math.Round((double)Total / PageSize, 0, MidpointRounding.ToPositiveInfinity));
    }
}