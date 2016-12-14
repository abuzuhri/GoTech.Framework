using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Paging
{
    public class IPagingList<T> : List<T>
    {
        //public PagingParamerters pagingParamerters { get; set; }
        public PaginationDetails PaginationDetails { get; set; }
        public IPagingList()
        {
        }
        public IPagingList(IQueryable<T> source, PagingParamerters pagingParamerters)
        {
            PaginationDetails.sort_column = pagingParamerters.sort_column;
            PaginationDetails.sort_type = pagingParamerters.sort_type;

            if (!isSortable(source))
            {
                string SortEx = pagingParamerters.sort_column + " " + pagingParamerters.sort_type;
                source = source.OrderBy(SortEx);
            }

            pagingParamerters.page_size=pagingParamerters.page_size <= 0 ? PagingParamerters.MAX_RESULT : pagingParamerters.page_size;

            if (pagingParamerters.pageing_type==PagingParamerters.PagingType.RegularPaging || (pagingParamerters.pageing_type == PagingParamerters.PagingType.SmartPaging && pagingParamerters.page == -1))
            {
                int totalItems = source.Count();
                PaginationDetails.items_count = totalItems;
                int totalPages = (int)Math.Ceiling((decimal)totalItems / pagingParamerters.page_size);
                if (totalPages < pagingParamerters.page)
                    pagingParamerters.page = totalPages;
                if (pagingParamerters.page == -1)
                    pagingParamerters.page = totalPages;
            }
            if (pagingParamerters.page < 1)
                pagingParamerters.page = 1;

            int firstPage = (pagingParamerters.page - 1) * pagingParamerters.page_size;
            
            AddRange(source.Skip(firstPage).Take(pagingParamerters.page_size + 1));
            if (Count > 0 && Count > pagingParamerters.page_size)
                RemoveAt(Count - 1);

            PaginationDetails.current_page = pagingParamerters.page_size;
            PaginationDetails.first_page = PagingParamerters.FIRST_PAGE;
            PaginationDetails.previous_page = pagingParamerters.page - 1 < PaginationDetails.first_page ? PaginationDetails.first_page : pagingParamerters.page - 1;
            PaginationDetails.next_page = pagingParamerters.page + 1;
            PaginationDetails.last_page = -1;

        }

        private bool isSortable(IQueryable<T> source)
        {
            string qeryStr = source.ToString();
            int pIndex = qeryStr.LastIndexOf(')');
            if (pIndex == -1)
                pIndex = 0;
            if (qeryStr.IndexOf("ORDER BY", pIndex) != -1)
                return true;
            else return false;
        }
    }
}
