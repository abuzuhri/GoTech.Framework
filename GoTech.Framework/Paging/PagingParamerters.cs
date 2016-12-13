using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Paging
{
    public class PagingParamerters
    {
        public const int MAX_RESULT=10;
        public const int FIRST_PAGE=1;
        public int page { get; set; } = FIRST_PAGE;
        public int page_size { get; set; } = MAX_RESULT;
        public PagingType pageing_type { get; set; } = PagingType.SmartPaging;
        public string sort_column { get; set; }
        public string sort_type { get; set; }

        public enum PagingType {
            SmartPaging,
            RegularPaging
        }

    }
}
