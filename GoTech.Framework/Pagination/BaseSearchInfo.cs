using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Pagination
{
    public class BaseSearchInfo
    {
        public int PageNo { get; set; }
        public string sort_column { get; set; }
        public string sort_type { get; set; }
        public int ItemsPerPage { get; set; }
        public bool isGoToPage { get; set; }
        public string GetSortColumn
        {
            get
            {
                return sort_column;
            }
        }
        public string GetSortType
        {
            get
            {
                if (string.IsNullOrEmpty(sort_type))
                {
                    return "ASC";
                }
                else if (sort_type.ToLower().Equals("desc"))
                    return "DESC";
                else return "ASC";
            }
        }
    }
}
