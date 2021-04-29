using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    public class Order //מחלקה שמייצגת הזמנה
    {
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }
        public Enums.OrderStatus orderStatus { get; set; } = Enums.OrderStatus.טרם_טופל;
        public DateTime CreateDate=DateTime.Now;
        public DateTime OrderDate;
        public override string ToString()
        {
            return this.ToStringProperty();
            //return string.Format("HostingUnitKey={0} /n, GuestRequestKey= {1} /n, OrderKey={2} /n, CreateDate={3} /n, " +
            //    "OrderDate={4} /n "
            //    , HostingUnitKey, GuestRequestKey, OrderKey, CreateDate, OrderDate);
        }

        public static implicit operator Order(XElement v)
        {
            throw new NotImplementedException();
        }
    }
}
