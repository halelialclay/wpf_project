using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    public class Configuration //מחלקה שכוללת את כל המשתנים הגלובליים כשדות סטטיים
    {
        public static int GuestRequestKey
        {
            get {return readFromConfig("GuestRequestKey");}
            set{writeToConfig("GuestRequestKey", value);}
        }

        public static int HostKey
        {
            get { return readFromConfig("HostKey"); }
            set { writeToConfig("HostKey", value); }
        }
        public static int HostingUnitKey
        {
            get { return readFromConfig("HostingUnitKey"); }
            set { writeToConfig("HostingUnitKey", value); }
        }
        public static int OrderKey
        {
            get { return readFromConfig("OrderKey"); }
            set { writeToConfig("OrderKey", value); }
        }
        public static int UserId
        {
            get { return readFromConfig("UserId"); }
            set { writeToConfig("UserId", value); }
        }

        public static int BankNumber = 1;
        public static int Commission = 10;
        public static int SeveralDaysToExpireOrder = 20;

        private static void writeToConfig(string attr, int value)
        {
            XDocument xmlDoc = XDocument.Load("config.xml");

            XElement item = (from el in xmlDoc.Descendants(attr)
             select el).First();

            item.SetValue(value);

            xmlDoc.Save("config.xml");
        }

        private static int readFromConfig(string attr)
        {
            XDocument xmlDoc = XDocument.Load("config.xml");

            return (from el in xmlDoc.Descendants(attr)
                         select Convert.ToInt32(el.Value)).First();
        }


    }
}
