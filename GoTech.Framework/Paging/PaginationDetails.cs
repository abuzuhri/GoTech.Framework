using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Paging
{
    public class PaginationDetails
    {
        //public PagingParamerters PagingParamerters { get;set;}
        public int current_page { get; set; }
        public int first_page { get; set; }
        public int previous_page { get; set; }
        public int next_page { get; set; }
        public int last_page { get; set; }
        public int? items_count { get; set; }
        public string sort_column { get; set; }
        public string sort_type { get; set; }
        public string setAsSoringCoulmn(string ColumnName)
        {
            if (!string.IsNullOrEmpty(ColumnName) && !string.IsNullOrEmpty(sort_column))
            {
                if (ColumnName.ToLower() == sort_column.ToLower())
                {
                    if (sort_type.ToUpper().Equals("ASC"))
                    {
                        return " sortable=sorting_asc sorting-column=" + ColumnName;
                    }
                    else return " sortable=sorting_desc sorting-column=" + ColumnName;
                }
            }
            return " sortable=sorting sorting-column=" + ColumnName; ;
        }
    }
}
