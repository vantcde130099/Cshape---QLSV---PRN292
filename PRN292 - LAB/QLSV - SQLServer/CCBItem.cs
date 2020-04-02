using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV___SQLServer
{
    class CCBItem
    {
        public String Text { get; set; }
        public String Value { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
