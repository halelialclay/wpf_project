using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using static BE.Enums;

namespace DAL
{
    public class Dal_XML_imp : Idal
    {
        public void WriteXml(object obj, string filename)
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            using (StreamWriter writer = new StreamWriter(filename))
            {
                s.Serialize(writer, obj);
            }
        }

        public T ReadXml<T>(string filename)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filename))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }

        private string GuestRequestsFile = "GuestRequests.xml";
        private string HostingUnitsFile = "HostingUnits.xml";
        private string OrdersFile = "Orders.xml";
        private string UsersFile = "Users.xml";
        private string BanksFile = "Banks.xml";
        private bool BanksLoaded = false;

        public Dal_XML_imp()
        {
            if (!File.Exists(GuestRequestsFile))
                WriteXml(DS.DataSource.guestrequest,GuestRequestsFile);

            if (!File.Exists(HostingUnitsFile))
                WriteXml(DS.DataSource.hostingunit,HostingUnitsFile);

            if (!File.Exists(OrdersFile))
                WriteXml(DS.DataSource.order,OrdersFile);

            if (!File.Exists(UsersFile))
                WriteXml(DS.DataSource.users,UsersFile);

            if (!File.Exists(BanksFile))
                LoadBankList();

            new Thread(() =>
            {
                do
                {
                    try
                    {
                        LoadBankList();
                        BanksLoaded = true;
                    }
                    catch (Exception)
                    {
                    }
                    
                    Thread.Sleep(2000);
                } while (!BanksLoaded);
            }).Start();
        }

        #region Bank
        private void LoadBankList()
        {
            const string xmlLocalPath = @"atm.xml";
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            catch (Exception)
            {
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            finally
            {
                wc.Dispose();
            }
            ConvertBankXml();
        }

        private void ConvertBankXml()
        {
            XElement xmlDoc = XElement.Load("atm.xml");

            List<string> snifim = new List<string>();

            List<BankAccount> accounts = new List<BankAccount>();

            foreach (XElement item in xmlDoc.Elements())
            {
                if (item.Element("קוד_סניף")!=null && !snifim.Contains(item.Element("קוד_סניף").Value))
                {
                    snifim.Add(item.Element("קוד_סניף").Value);
                    BankAccount account = new BankAccount();
                    account.BranchAddress = item.Element("כתובת_ה-ATM").Value;
                    account.BankName = item.Element("שם_בנק").Value;
                    account.BankNumber =Convert.ToInt32( item.Element("קוד_בנק").Value);
                    account.BranchCity = item.Element("ישוב").Value;
                    account.BranchNumber = Convert.ToInt32(item.Element("קוד_סניף").Value);
               
                    accounts.Add(account);
                }   
            }
            WriteXml(accounts, BanksFile);
        }

        //אח"כ לרשימת בנקים להחזיר
        public List<BankAccount> GetListBankAccount()
        {
            List<BankAccount> list = ReadXml<List<BankAccount>>(BanksFile);
            return list;
        }

        #endregion Bank

        #region Orders
        public int CreateOrder(Order o)
        {
            if (o.OrderKey == 0)
            {
                int id = ++BE.Configuration.OrderKey;
                ((Order)o).OrderKey = id;
            }
            List<Order> list = ReadAllOrder();
            list.Add((Order)o);
            WriteXml(list, "Orders.xml");
            return o.OrderKey;
        }

        public bool DeleteOrder(int Id)
        {
            List<Order> list = ReadAllOrder();
            Order order = list.Where(h => h.OrderKey == Id).FirstOrDefault();

            if (order == null)
                throw new BE.ZimmerException("Order Not Found");
            else
            {
                list.Remove(order);
                WriteXml(list, OrdersFile);
                return true;
            }
        }

        public List<Order> ReadAllOrder()
        {
            //List<Order> list = ReadXml<List<Order>>(OrdersFile);
            XDocument xmlDoc = XDocument.Load(OrdersFile);

            List<Order> list = (from el in xmlDoc.Descendants("Order")
                        select new Order
                        {
                            CreateDate = Convert.ToDateTime(el.Element("CreateDate").Value),
                            GuestRequestKey = Convert.ToInt32(el.Element("GuestRequestKey").Value),
                            HostingUnitKey = Convert.ToInt32(el.Element("HostingUnitKey").Value),
                            OrderKey = Convert.ToInt32(el.Element("OrderKey").Value),
                            orderStatus = (BE.Enums.OrderStatus)Enum.Parse(typeof(BE.Enums.OrderStatus), el.Element("orderStatus").Value),
                            OrderDate = Convert.ToDateTime(el.Element("OrderDate").Value)
                        }
                ).ToList();

            return list;
        }

        public Order ReadOrder(int order)
        {
            XDocument xmlDoc = XDocument.Load(OrdersFile);

            Order src = (from el in xmlDoc.Descendants("Order")
                    where el.Element("OrderKey").Value  == order.ToString()
                    select new Order
                    {
                       CreateDate = Convert.ToDateTime(el.Element("CreateDate").Value),
                       GuestRequestKey = Convert.ToInt32(el.Element("GuestRequestKey").Value),
                       HostingUnitKey = Convert.ToInt32(el.Element("HostingUnitKey").Value),
                       OrderKey = Convert.ToInt32(el.Element("OrderKey").Value),
                       orderStatus = (BE.Enums.OrderStatus)Enum.Parse(typeof(BE.Enums.OrderStatus),el.Element("orderStatus").Value),
                       OrderDate = Convert.ToDateTime(el.Element("OrderDate").Value)
                    }
                ).FirstOrDefault();

            /*Order src = list
                .Where(h => h.OrderKey == order)
                  .FirstOrDefault();
                  */
            if (src != null)
                return src;
            else
                throw new BE.ZimmerException("Not Found");
        }

        public bool UpdateOrder(Order order)
        {
            List<Order> list = ReadAllOrder();
            Order target = list
                                .Where(h => h.OrderKey == ((Order)order).OrderKey)
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
        #endregion

        #region Guests
        public int CreateGuestRequest(GuestRequest guest)
        {
            if (guest.GuestRequestKey == 0)
            {
                int id = ++BE.Configuration.GuestRequestKey;
                guest.GuestRequestKey = id;
            }         
            List<GuestRequest> list = ReadXml<List<GuestRequest>>(GuestRequestsFile);
            list.Add(guest);
            WriteXml(list, GuestRequestsFile);
            return guest.GuestRequestKey;
        }

        public bool DeleteGuestRequest(int Id)
        {
            List<GuestRequest> list = ReadXml<List<GuestRequest>>(GuestRequestsFile);
            GuestRequest guestRequest = list.Where(h => h.GuestRequestKey == Id).First();

            if (guestRequest == null)
                throw new BE.ZimmerException("GuestRequest Not Found");
            else
            {
                list.Remove(guestRequest);
                WriteXml(list, GuestRequestsFile);
                return true;
            }
        }

        public List<GuestRequest> ReadAllGuestRequest()
        {
            List<GuestRequest> list = ReadXml<List<GuestRequest>>(GuestRequestsFile);
            return list;
        }

        public GuestRequest ReadGuestRequest(int Id)
        {
            List<GuestRequest> list = ReadAllGuestRequest();
            GuestRequest src = list.Where(h => h.GuestRequestKey == Id).FirstOrDefault();
            if (src != null)
                return src;
            else
                throw new BE.ZimmerException("Not Found");
        }

        public bool UpdateGuestRequest(GuestRequest guestRequest)
        {
            List<GuestRequest> list = ReadAllGuestRequest();
            GuestRequest target = list
                       .Where(h => h.GuestRequestKey == ((GuestRequest)guestRequest).GuestRequestKey).FirstOrDefault();
            if (target == null)
                throw new BE.ZimmerException("This hosting unit has been found");
            else
            {
                DeleteGuestRequest(target.GuestRequestKey);
                CreateGuestRequest(guestRequest);

                return true;
            }
        }

        #endregion Guests

        #region HostingUnit

        public int CreateHostingUnit(HostingUnit hostunit, Host host)
        {
            if (hostunit.HostingUnitKey == 0)
            {
                int idHostingUnit = ++BE.Configuration.HostingUnitKey;
                if (host.HostKey == 0)
                    host.HostKey = ++BE.Configuration.HostKey;
                hostunit.HostingUnitKey = idHostingUnit;
                hostunit.Owner = host;
            }
            List<HostingUnit> list = ReadXml<List<HostingUnit>>(HostingUnitsFile);
            list.Add(hostunit);
            WriteXml(list, HostingUnitsFile);
            return hostunit.HostingUnitKey;
        }

        public bool DeleteHostingUnit(int Id)
        {
            List<HostingUnit> list = ReadXml<List<HostingUnit>>(HostingUnitsFile);
            HostingUnit hostunit = list.Where(h => h.HostingUnitKey == Id).FirstOrDefault();
            if (hostunit == null)
                throw new BE.ZimmerException("HostUnit Not Found");
            else
            {
                list.Remove(hostunit);
                WriteXml(list, HostingUnitsFile);
                return true;
            }
        }

        public List<HostingUnit> ReadAllHostingUnit()
        {
            List<HostingUnit> list = ReadXml<List<HostingUnit>>(HostingUnitsFile);
            return list;
        }

        public HostingUnit ReadHostingUnit(int Id)
        {
            List<HostingUnit> list = ReadAllHostingUnit();
            HostingUnit src = list.Where(h => h.HostingUnitKey == Id).FirstOrDefault();
            if (src != null)
                return src;
            else
                throw new BE.ZimmerException("Not Found");
        }

        public bool UpdateHostingUnit(HostingUnit hosting)
        {
            List<HostingUnit> list = ReadAllHostingUnit();
            HostingUnit target = list
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

        #endregion

        #region User

        public User GetUser(string lUserName, string lPassword)
        {
            List<User> list = ReadXml<List<User>>(UsersFile);
            return list.Where(u => u.Username == lUserName && u.Password == lPassword).FirstOrDefault();
        }

        public List<User> ReadAllUser()
        {
            List<User> list = ReadXml<List<User>>(UsersFile);
            return list;
        }

        public void updateUser(int Id, int refId)
        {
            List<User> list = ReadAllUser();
            var temp = list.Where(e => e.Id == Id).FirstOrDefault();
            temp.RefId = refId;
            WriteXml(list, UsersFile);
        }

        #endregion

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
            List<User> list = ReadAllUser();
            var temp = list.Where(e => e.Username == ruser.Username).FirstOrDefault();
            if (temp == null)
            {
                int id = ++BE.Configuration.UserId;
                ruser.Id = id;
                list.Add(ruser);
                WriteXml(list, UsersFile);
                return true;
            }
            return false;
        }

        public bool DeleteUser(int Id)
        {
            List<User> list = ReadAllUser();
            User user = list.Where(h => h.Id == Id).FirstOrDefault();
            if (user == null)
                throw new BE.ZimmerException("Order Not Found");
            else
            {
                list.Remove(user);
                WriteXml(list, UsersFile);
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