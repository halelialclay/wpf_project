using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BE;
using static BE.Enums;

namespace DAL
{
    public class Dal_imp : Idal
    {      
        public int CreateGuestRequest(GuestRequest guest)
        {
            if (guest.GuestRequestKey == 0)
            {
                int id = ++BE.Configuration.GuestRequestKey;
                guest.GuestRequestKey = id;
            }
            
            DS.DataSource.guestrequest.Add(guest);
            return guest.GuestRequestKey;
        }

        public void SerializeListGuest(List<GuestRequest> list)
        {
            //GuestRequestsRoot.Save(GuestRequestsRootPath);

            XmlSerializer xsser = new XmlSerializer(typeof(List<GuestRequest>));
            string xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsser.Serialize(writer, list);
                    xml = sww.ToString();
                }
            }
        }

        public int CreateHostingUnit(HostingUnit hostunit, Host host)
        {

            if (hostunit.HostingUnitKey == 0)
            {
               // diary(hostunit);
                int idHostingUnit = ++BE.Configuration.HostingUnitKey;
                int idHost = ++BE.Configuration.HostKey;
            }
            hostunit.HostingUnitKey = hostunit.HostingUnitKey;
            DS.DataSource.hostingunit.Add(hostunit);
            return hostunit.HostingUnitKey;
        }

        public int CreateOrder(Order o)
        {

            if (o.OrderKey == 0)
            {
                int id = ++BE.Configuration.OrderKey;
                ((Order)o).OrderKey = id;
            }
            DS.DataSource.order.Add((Order)o);
            return o.OrderKey;
        }

        public bool DeleteGuestRequest(int Id)
        {
            GuestRequest guestRequest = DS.DataSource.guestrequest.Where(h => h.GuestRequestKey == Id).First();

            if (guestRequest == null)
                throw new BE.ZimmerException("GuestRequest Not Found");
            else
            {
                DS.DataSource.guestrequest.Remove(guestRequest);
                return true;
            }
        }

        public User GetUser(string lUserName, string lPassword)
        {
            return DS.DataSource.users.Where(u => u.Username == lUserName && u.Password == lPassword).FirstOrDefault();
        }

        public bool DeleteHostingUnit(int Id)
        {
            HostingUnit hostunit = DS.DataSource.hostingunit.Where(h => h.HostingUnitKey == Id).FirstOrDefault();

            if (hostunit == null)
                throw new BE.ZimmerException("HostUnit Not Found");
            else
            {
                DS.DataSource.hostingunit.Remove(hostunit);
                return true;
            }
        }

        public bool DeleteOrder(int Id)
        {
            Order order = DS.DataSource.order.Where(h => h.OrderKey == Id).FirstOrDefault();

            if (order == null)
                throw new BE.ZimmerException("Order Not Found");
            else
            {
                DS.DataSource.order.Remove(order);
                return true;
            }
        }

        public List<HostingUnit> ReadAllHostingUnit()
        {
            HostingUnit[] temp = new HostingUnit[DS.DataSource.hostingunit.Count];
            DS.DataSource.hostingunit.CopyTo(temp);

            if (temp == null)
                throw new BE.ZimmerException("No accommodation available");
            return temp.ToList<HostingUnit>();
        }

        public List<Order> ReadAllOrder()
        {
            Order[] temp = new Order[DS.DataSource.order.Count];
            DS.DataSource.order.CopyTo(temp);
            if (temp == null)
                throw new BE.ZimmerException("No orders are available");
            return temp.ToList<Order>();
        }

        public List<GuestRequest> ReadAllGuestRequest()
        {
            GuestRequest[] temp = new GuestRequest[DS.DataSource.guestrequest.Count];
            DS.DataSource.guestrequest.CopyTo(temp);
            if (temp == null)
                throw new BE.ZimmerException("No customers exist");
            return temp.ToList<GuestRequest>();
        }

        public GuestRequest ReadGuestRequest(int Id)
        {
            GuestRequest src = DS.DataSource.guestrequest
                       .Where(h => h.GuestRequestKey == Id).FirstOrDefault();

            if (src != null)
                return Copy(src);
            else
                throw new BE.ZimmerException("Not Found");
        }

        public HostingUnit ReadHostingUnit(int Id)
        {
            HostingUnit src = DS.DataSource.hostingunit
                .Where(h => h.HostingUnitKey == Id)
                  .First();
            if (src != null)
                return Copy(src);
            else
                throw new BE.ZimmerException("Not Found");
        }

        public Order ReadOrder(int order)
        {
            Order src = DS.DataSource.order
                .Where(h => h.OrderKey == order)
                  .FirstOrDefault();
            if (src != null)
                return (Order)Copy(src);
            else
                throw new BE.ZimmerException("Not Found");
        }

        public bool UpdateGuestRequest(GuestRequest guestRequest)
        {

            GuestRequest target = DS.DataSource.guestrequest
                       .Where(h => h.GuestRequestKey == ((GuestRequest)guestRequest).GuestRequestKey).FirstOrDefault();
            if (target == null)
                throw new BE.ZimmerException("This hosting unit has been found");
            else
            {
                DeleteGuestRequest(target.GuestRequestKey);
                CreateGuestRequest( guestRequest);
               
                return true;
            }
        }

        public bool UpdateHostingUnit(HostingUnit hosting)
        {
            HostingUnit target = DS.DataSource.hostingunit
                                .Where(h => h.HostingUnitKey == ((HostingUnit)hosting).HostingUnitKey)
                                .FirstOrDefault();
            if (hosting == null)
                throw new BE.ZimmerException("This hosting unit has been found");
            else
            {
                DeleteHostingUnit(target.HostingUnitKey);
                CreateHostingUnit(hosting, hosting.Owner);
                return true;
            }
        }

        public bool UpdateOrder(Order order)
        {
            Order target = DS.DataSource.order
                                .Where(h => h.HostingUnitKey == ((Order)order).OrderKey)
                                .FirstOrDefault();
            if (order == null)
                throw new BE.ZimmerException("This hosting unit has been found");
            else
            {
                DeleteOrder(target.OrderKey);
                CreateOrder(order);
                return true;
            }
        }

        //אח"כ לרשימת בנקים להחזיר
        public List<BankAccount> GetListBankAccount()
        {
            BankAccount[] temp = new BankAccount[DS.DataSource.banks.Count];
            DS.DataSource.banks.CopyTo(temp);
            return temp.ToList();
        }

        public ObjectType Copy<ObjectType>(ObjectType src)
        {
            ObjectType target = (ObjectType)Activator.CreateInstance(src.GetType());
            foreach (PropertyInfo item in src.GetType().GetProperties())
            {
                item.SetValue(target, item.GetValue(src));
            }
            return target;
        }

        public void diary(HostingUnit hosting)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    hosting.Diary[i, j] = false;
                }
            }
        }

        public bool register(User ruser)
        {
            var temp = DS.DataSource.users.Where(e => e.Username == ruser.Username).FirstOrDefault();
            if (temp==null)
            {
                int id = ++BE.Configuration.UserId;
                ruser.Id= id;
                DS.DataSource.users.Add(ruser);
                return true;
            }
            return false;
          
        }
        public bool DeleteUser(int Id)
        {
            User user= DS.DataSource.users.Where(h => h.Id == Id).FirstOrDefault();

            if (user == null)
                throw new BE.ZimmerException("Order Not Found");
            else
            {
                DS.DataSource.users.Remove(user);
                return true;
            }
        }

        public void SerializeList<T>(List<T> list)
        {
            XmlSerializer xsser = new XmlSerializer(typeof(T));
            string xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsser.Serialize(writer, list);
                    xml = sww.ToString();
                }
            }
        }
    }
}