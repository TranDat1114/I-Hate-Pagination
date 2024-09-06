using Microsoft.EntityFrameworkCore;
namespace Common
{
    public class PaginationModel<T>
    {
        public PaginationModel()
        {

        }
        public PaginationModel(int count, int pageNumber, int pageSize, IEnumerable<T>? items)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            Items = items;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T>? Items { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    public static class IQueryableExtensions
    {
        public static async Task<PaginationModel<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationModel<T>(count, pageNumber, pageSize, items);
        }
        public static PaginationModel<T> ToPagedList<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PaginationModel<T>(count, pageNumber, pageSize, items);
        }
    }
    public class PaginationDTO
    {
        public PaginationDTO()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public PaginationDTO(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}