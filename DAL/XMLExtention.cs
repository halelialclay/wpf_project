using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public static class XMLExtention
    {
        //private static string filePath = c @"..\..\..\xml files\";

        //public static void WriteXml(this object obj, string filename)
        //{
        //    XmlSerializer s = new XmlSerializer(obj.GetType());
        //    using (StreamWriter writer = new StreamWriter(filename))
        //    {
        //        s.Serialize(writer, obj);
        //    }
        //}

        //public static T ReadXml<T>(this object data, string filename)
        //{
        //    XmlSerializer s = new XmlSerializer(typeof(T));
        //    using (StreamReader reader = new StreamReader(filename))
        //    {
        //        object obj = s.Deserialize(reader);
        //        return (T)obj;
        //    }
        //}
    }
}