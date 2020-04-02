using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV_Query
{
    class SV
    {
        public string MSSV { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public double DTB { get; set; }
        public string Tel { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public bool CMND { get; set; }
        public bool HocBa { get; set; }
        public bool THPT { get; set; }

        public static bool NameCompareTo(object o1, object o2)
        {
            if (string.Compare(((SV)o1).Name, ((SV)o2).Name) > 0)
            {
                return true;
            }
            else
                return false;
        }
        public static bool DTBCompareTo(object o1, object o2)
        {
            if ((((SV)o1).DTB > ((SV)o2).DTB))
            {
                return true;
            }
            else
                return false;
        }
        public static bool BithdayCompareTo(object o1, object o2)
        {
            if (DateTime.Compare(((SV)o1).Birthday, ((SV)o2).Birthday) > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
