using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Paging
{
    public class PaginationDetails
    {
        public int current_page { get; set; }
        public int first_page { get; set; }
        public int previous_page { get; set; }
        public int next_page { get; set; }
        public int last_page { get; set; }
        public int? items_count { get; set; }
    }
}
