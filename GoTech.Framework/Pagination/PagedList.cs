using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Pagination
{
    public class PagedList<T> : List<T>
    {
        public PagingInfo pagingInfo { get; set; }
        public IQueryable<T> Query { get; set; }

        private const int MAX_ITEM = 10;
        public PagedList()
        {
            pagingInfo = new PagingInfo();
            pagingInfo.TotalItemsReturns = MAX_ITEM;
        }
        public PagedList(IQueryable<T> source, BaseSearchInfo baseSearchInfo)
        {
            Query = source;
            //if (!isSortable)
            //{
            //    string SortEx = baseSearchInfo.GetSortColumn + " " + baseSearchInfo.GetSortType;
            //    //source = source.OrderBy(SortEx);
            //}

            pagingInfo = new PagingInfo(baseSearchInfo);
            pagingInfo.ItemsPerPage = baseSearchInfo.ItemsPerPage <= 0 ? MAX_ITEM : baseSearchInfo.ItemsPerPage;
            pagingInfo.CurrentPage = baseSearchInfo.PageNo;

            if (baseSearchInfo.isGoToPage || pagingInfo.CurrentPage == -1)
            {
                int totalItems = source.Count();
                int totalPages = (int)Math.Ceiling((decimal)totalItems / pagingInfo.ItemsPerPage);
                if (totalPages < pagingInfo.CurrentPage)
                    pagingInfo.CurrentPage = totalPages;
                if (pagingInfo.CurrentPage == -1)
                    pagingInfo.CurrentPage = totalPages;
            }
            if (pagingInfo.CurrentPage < 1)
                pagingInfo.CurrentPage = 1;

            AddRange(source.Skip(pagingInfo.FirstIndex).Take(pagingInfo.ItemsPerPage + 1));
            pagingInfo.TotalItemsReturns = Count;
            if (Count > 0 && pagingInfo.TotalItemsReturns > pagingInfo.ItemsPerPage)
                RemoveAt(Count - 1);
        }


        private bool isSortable
        {
            get
            {
                string qeryStr = Query.ToString();
                int pIndex = qeryStr.LastIndexOf(')');
                if (pIndex == -1)
                    pIndex = 0;
                if (qeryStr.IndexOf("ORDER BY", pIndex) != -1)
                    return true;
                else return false;
            }
        }
    }

}
