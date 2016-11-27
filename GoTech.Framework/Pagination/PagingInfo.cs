using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Pagination
{
    public class PagingInfo
    {
        private BaseSearchInfo baseSearchInfo { get; set; }
        public int TotalItemsReturns { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public PagingInfo() { }
        public PagingInfo(BaseSearchInfo baseSearchInfo)
        {
            this.baseSearchInfo = baseSearchInfo;
        }

        public int FirstIndex
        {
            get
            {
                int inx = (CurrentPage - 1) * ItemsPerPage;
                if (inx < 0)
                    return 0;
                else return inx;
            }
        }
        private int FirstRowCount
        {
            get
            {
                return FirstIndex + 1;
            }
        }

        private string OnClickButton(string name)
        {
            if (name.Equals("first"))
            {
                if (FirstRowCount == 1)
                    return "";
                else return " onclick='Pager(1);' ";
            }
            else if (name.Equals("previous"))
            {
                if (FirstRowCount == 1)
                    return "";
                else return " onclick='Pager(" + (CurrentPage - 1) + ");' ";
            }
            else if (name.Equals("next"))
            {
                if (ItemsPerPage >= TotalItemsReturns)
                    return "";
                else return " onclick='Pager(" + (CurrentPage + 1) + ");' ";
            }
            else if (name.Equals("last"))
            {
                if (ItemsPerPage >= TotalItemsReturns)
                    return "";
                else return " onclick='Pager(" + (-1) + ");' ";
            }
            else if (name.Equals("page"))
            {
                if (ItemsPerPage > TotalItemsReturns)
                    return "disabled";
                else return "";
            }
            else return "";
        }
        private string hasButton(string name)
        {
            if (name.Equals("first"))
            {
                if (FirstRowCount == 1)
                    return "disabled";
                else return "";
            }
            else if (name.Equals("previous"))
            {
                if (FirstRowCount == 1)
                    return "disabled";
                else return "";
            }
            else if (name.Equals("next"))
            {
                if (ItemsPerPage >= TotalItemsReturns)
                    return "disabled";
                else return "";
            }
            else if (name.Equals("last"))
            {
                if (ItemsPerPage >= TotalItemsReturns)
                    return "disabled";
                else return "";
            }
            else if (name.Equals("page"))
            {
                if (ItemsPerPage > TotalItemsReturns && CurrentPage == 1)
                    return "disabled";
                else return "";
            }
            else return "";
        }
        public string isSoringCoulmn(string ColumnName)
        {
            if (!string.IsNullOrEmpty(ColumnName) && !string.IsNullOrEmpty(baseSearchInfo.sort_column))
            {
                if (ColumnName.ToLower() == baseSearchInfo.sort_column.ToLower())
                {
                    if (baseSearchInfo.GetSortType.Equals("ASC"))
                    {
                        return " sortable=sorting_asc sorting-column=" + ColumnName;
                    }
                    else return " sortable=sorting_desc sorting-column=" + ColumnName;
                }
            }
            return " sortable=sorting sorting-column=" + ColumnName; ;
        }


        public string PageLinks
        {
            get
            {

                //                < ul class="pagination">
                //  <li><a href = '#' > 1 </a></li>
                //  <li><a href='#'>2</a></li>
                //  <li><a href = '#' > 3 </a></li>
                //  <li class='disabled'><a href = '#'>4</a></li>
                //   <li><a href='#'>5</a></li>
                //</ul>

                StringBuilder result = new StringBuilder(@"<div class='footer'><ul class='pagination'>
<li class='" + hasButton("first") + @"' ><a href = 'javascript:void()' " + OnClickButton("first") + @" >First</a></li>
<li class='" + hasButton("previous") + @"'><a href = 'javascript:void()' " + OnClickButton("previous") + @" >Previous</a></li>
<li class='" + hasButton("next") + @"'><a href = 'javascript:void()' " + OnClickButton("next") + @" >Next</a></li>
<li class='" + hasButton("last") + @"'><a href = 'javascript:void()'  " + OnClickButton("last") + @" >Last</a></li>
</ul></div>");
                return result.ToString();
            }
        }
    }
}
