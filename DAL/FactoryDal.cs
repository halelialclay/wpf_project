using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class FactoryDal
    {
        static Dal_XML_imp dal = null;
        public static Dal_XML_imp GetIdal()
        {
            if (dal == null)
                dal = new Dal_XML_imp();
            return dal;
        }
    }
}