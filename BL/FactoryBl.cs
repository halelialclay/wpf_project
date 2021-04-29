using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FactoryBl
    {
        static BL_imp bl = null;
        public static BL_imp GetIBL()
        {
            bl = new BL_imp();
            return bl;
        }
    }
}
