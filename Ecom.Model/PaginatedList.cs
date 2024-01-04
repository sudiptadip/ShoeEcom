using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItem { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            TotalItem = items.Count();
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(items.Count() / (double)pageSize);
            Items = items.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public int FirstIndexItem => (PageNumber - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(PageNumber * PageSize, TotalItem);



        //public static Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        //{
        //    var count =  source.Count();
        //    var items = source.Skip((pageIndex - 1) *  pageSize).Take(count);
        //    return new PaginatedList<T>(items, count, pageIndex, pageSize);
        //}

    }
}
