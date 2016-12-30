using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Service
{
    public class BsinessRulesException : Exception
    {
        public BsinessRulesException(string msg) : base(msg) { }
    }
}
