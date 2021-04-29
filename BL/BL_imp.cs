using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public class BL_imp : IBL
    {
        private Dal_XML_imp dal =  FactoryDal.GetIdal();
        //private Dal_imp dal = FactoryDal.GetIdal();
        public static User loggedInUser;
        public static Host loggedInHost;

        public BL_imp ()
        {
            new Thread(() =>
                {
                    do
                    {
                        var orders = dal.ReadAllOrder();
                        foreach (var item in orders)
                        {
                            if (item.orderStatus == Enums.OrderStatus.נשלח_מייל && item.OrderDate > item.OrderDate.AddDays(30))
                            {
                                item.orderStatus = BE.Enums.OrderStatus.פג_תוקפה;
                                UpdateOrder(item);
                            }
                        }

                        Thread.Sleep(1000 * 60 * 60 * 24);
                    } while (true);
                }).Start();
        }

        #region GuestRequest
        public int CreateGuestRequest(GuestRequest t) //פונקציה להוספת דרישת לקוח
        {
            if (t.area == Enums.Area.Jerusalem && t.WindowToSea == Enums.enum_1.הכרחי)
                throw new BE.ZimmerException("Surrey in the sea in Jerusalem");
            if (checkDate(t.EntryDate, t.ReleaseDate))
                return (dal.CreateGuestRequest(t));

            else throw new BE.ZimmerException("The hospitality requirement is invalid");
        }
        public bool DeleteGuestRequest(int Id) //פונקציה למחיקת דרישת לקוח
        {
            dal = DAL.FactoryDal.GetIdal();
            try
            {
                return dal.DeleteGuestRequest(Id);
            }
            catch (BE.ZimmerException a)
            {
                throw a;
            }
        }

        #endregion

        #region HostingUnit
        public List<HostingUnit> GetHostingUnits(int hostKey) //פונקציה שניגשת ליחידה מסוימת לפי מזהה
        {
            return dal.ReadAllHostingUnit().Where(unit => unit.Owner.HostKey == hostKey).ToList();
        }
        public int CreateHostingUnit(HostingUnit t, Host host) // פונקציה להוספת יחידת אירוח
        {
            if (t.area == Enums.Area.Jerusalem && t.WindowToTheSea == Enums.Yes_No.Yes)
                throw new BE.ZimmerException("Rogue has no sea in Jerusalem");
            return dal.CreateHostingUnit(t, host);
        }

        #endregion


        public User Login(string lUserName, string lPassword) //?
        {
            
            User user = dal.GetUser(lUserName, lPassword); 
            return user;
        }

        public int CreateOrder(Order order) //פונקציה להוספת הזמנה
        {
            try
            {
                bool a = Search(order);
                bool b = checkDays(order);
            }
            catch (BE.ZimmerException a)
            {
                throw a;
            }
            return dal.CreateOrder(order);
        }

       public List<User> ReadAllUser()
        {
            return dal.ReadAllUser();
        }
        public bool register(User ruser) //?
        {
            return dal.register(ruser);
        }

        public bool DeleteHostingUnit(int Id) //פונקציה למחיקת יחידת אירוח
        {
            dal = DAL.FactoryDal.GetIdal();
            List<Order> tempList = dal.ReadAllOrder().Where(x => x.HostingUnitKey == Id).ToList();
            foreach (var item in tempList)
            {
                if (dal.ReadGuestRequest(item.GuestRequestKey).status == Enums.CustomerRequirementStatus.open)
                {
                    throw new ZimmerException("The unit cannot be deleted because there is an open offer");
                }
            }        
            return dal.DeleteHostingUnit(Id);
        }

        public bool DeleteOrder(int Id) //פונקציה למחיקת הזמנה
        {
            dal = DAL.FactoryDal.GetIdal();
            try
            {
                return dal.DeleteOrder(Id);
            }
            catch (ZimmerException a )
            {
                throw a;
            }
            
        }

        public List<GuestRequest> ReadAllGuestRequest() //פונקציה שמציגה את רשימת דרישות הלקוח
        {
            dal = DAL.FactoryDal.GetIdal();
            try
            {
                return dal.ReadAllGuestRequest();
            }
            catch (ZimmerException x)
            {
                throw x;
            }
           
        }

        public List<HostingUnit> ReadAllHostingUnit() //פונקציה שמציגה את רשימת יחידות האירוח
        {
            dal = DAL.FactoryDal.GetIdal();
            try
            {
                return dal.ReadAllHostingUnit();
            }
            catch (ZimmerException x)
            {
                throw x;
            }
             
        }

        public List<Order> ReadAllOrder() //פונקציה שמציגה את רשימת ההזמנות
        {
            dal = DAL.FactoryDal.GetIdal();
            try
            {
                return dal.ReadAllOrder();
            }
            catch (ZimmerException x)
            {
                throw x;
            }
           
        }

        public GuestRequest ReadGuestRequest(int Id) //פונקציה שמציגה דרישת לקוח
        {
            try
            {
                return dal.ReadGuestRequest(Id);
            }
            catch (ZimmerException x)
            {
                throw new ZimmerException (x.Message);
            }
           
        }

        public HostingUnit ReadHostingUnit(int Id) //פונקציה שמציגה יחידת אירוח
        {
            try
            {
                return( dal.ReadHostingUnit(Id));
            }
            catch (ZimmerException x)
            {
                throw x;
            }
          
        }
        public bool DeleteUser(int Id)
        {
            return dal.DeleteUser(Id);
        }

        public Order ReadOrder(int order) //פונקציה שמציגה הזמנה
        {
            try
            {
                return dal.ReadOrder(order);
            }
            catch (ZimmerException x)
            {
                throw x;
            }
            
        }

        public bool UpdateGuestRequest(GuestRequest guestRequest) //פונקציה שמעדכנת דרישת לקוח
        {
            if (checkDate(guestRequest.EntryDate, guestRequest.ReleaseDate))
                return dal.UpdateGuestRequest(guestRequest);
            else
                return false;

        }

        public bool UpdateHostingUnit(HostingUnit t) //פונקציה שמעדכנת יחידת אירוח
        {
            dal = DAL.FactoryDal.GetIdal();

            if (dal.ReadHostingUnit(t.HostingUnitKey).Owner.CollectionClearance == Enums.Yes_No.Yes && t.Owner.CollectionClearance == Enums.Yes_No.No)
            {
                List<Order> tempList = dal.ReadAllOrder().Where(x => x.HostingUnitKey == t.HostingUnitKey).ToList();
                foreach (var item in tempList)
                {
                    if (dal.ReadGuestRequest(item.GuestRequestKey).status == Enums.CustomerRequirementStatus.open)
                    {
                        throw new ZimmerException("Bank account debit authorization cannot be revoked because there is an open offer");
                    }
                }
            }
            return dal.UpdateHostingUnit(t);
        }

        public void updateUser(int id, int refId)
        {
            dal.updateUser(id, refId);
        }

        public bool UpdateOrder(Order t) //פונקציה שמעדנכת הזמנה
        {
            dal = DAL.FactoryDal.GetIdal();
            if (t.orderStatus != dal.ReadOrder(t.OrderKey).orderStatus)
            {
                if (checkStatus(dal.ReadOrder(t.OrderKey)))
                    throw new ZimmerException("The status cannot be changed after a transaction is closed");
                if (checkStatus(t))
                {
                    commission(t);
                    guestStatus(t, Enums.OrderStatus.סגירת_עיסקה);
                    ChangesStatusAllCustomerOrders(t);
                    ClosingConfirmation(t);
                }
                if (checkStatusMaile(dal.ReadOrder(t.OrderKey)))
                {
                    if (!(dal.ReadHostingUnit(t.HostingUnitKey).Owner.CollectionClearance == Enums.Yes_No.Yes))
                    {
                        throw new ZimmerException("Mail cannot be sent until the collection confirmation is disapproved");
                    }
                }
            }
            return dal.UpdateOrder(t);
        }

        public List<BankAccount> GetListBankAccount() // פןנקציה שמחזירה רשימת סניפי בנק
        {
            return dal.GetListBankAccount();
        }

        public List<HostingUnit> AccommodationUnitsAvailable(DateTime date, int numberOfDays) //פונקציה שבודקת יחידות פנויות לפי תאריך
        {
            List<HostingUnit> hostingUnits = dal.ReadAllHostingUnit();
            List<HostingUnit> freeHostingnits = new List<HostingUnit>();
            foreach (var item in hostingUnits)
            {
                for (int i = 0; i < numberOfDays; i++)
                {
                    if (item.Diary[date.Month - 1, date.Day - 1] == true)
                    {
                        break;
                    }
                    else
                    {
                        date.AddDays(1);
                    }
                    if (i == numberOfDays - 1)
                    {
                        freeHostingnits.Add(item);
                    }
                }
            }
            if (freeHostingnits != null)
                return freeHostingnits;
            else throw new ZimmerException("No accommodation available");
        }
       
        public int numberOfOrdersForGuest(GuestRequest g) //פונקציה שמחזירה מספר הזמנות ללקוח
        {
            int orders = dal.ReadAllOrder().Where(x => x.GuestRequestKey == g.GuestRequestKey).Count();
            return orders;
        }

        public int numberOfOrdersForHostingUnit(HostingUnit h) //פונקציה שמחזירה מספר של הזמנות שקשורות ליחידת אירוח
        {
            List<Order> orders = dal.ReadAllOrder().Where(x => x.HostingUnitKey == h.HostingUnitKey).ToList();
            int count = orders.Where(y => y.orderStatus == Enums.OrderStatus.נסגר_מחוסר_הענות_הלקוח).Count();
            int numberOfOrders = orders.Count();
            return numberOfOrders - count;
        }

        public List<Order> TheWaitingTimeExpired(int numberOfDays)
        {
            dal = DAL.FactoryDal.GetIdal();
            List<Order> orders = dal.ReadAllOrder();
            List<Order> orders1 = new List<Order>();
            foreach (var item in orders)
            {
                if (dates(item.CreateDate) >= numberOfDays)

                    orders1.Add(item);

                else if (dates(item.OrderDate) >= numberOfDays)
                    orders1.Add(item);
            }
            return orders1;
        }
       
        public List<HostingUnit> UnitsInJerusalemWithPoolAndDisabledAccess() // פונקציה שמחזירה רשימה של יחידות בירושלים שנגישות לנכים 
        {
            dal = DAL.FactoryDal.GetIdal();
            List<HostingUnit> hostings = dal.ReadAllHostingUnit().Where(x => x.area == Enums.Area.Jerusalem).ToList();
            List<HostingUnit> hostings1 = hostings.Where(y => y.ThereIsPool == Enums.Yes_No.Yes).ToList();
            List<HostingUnit> hostings2 = hostings1.Where(z => z.DisabledAccessible == Enums.Yes_No.Yes).ToList();
            return hostings2;
        }
        
        public List<GuestRequest> AllGuestsWithPool()   /*88888888888888888*/ /*מימוש של delget*/
        {
            List<GuestRequest> pool = GetGuests(guestPool, (int)Enums.enum_1.הכרחי);
            return pool;
        }

        public delegate bool filter(GuestRequest gu, int val);

        public static bool guestPool(GuestRequest gu, int val) // בדיקה אם בדרישה יש בריכה 
        {
            return (int)gu.pool == val;
        }

        public static bool guestArea(GuestRequest gu, int val) // בדיקה אם באזור 
        {
            return (int)gu.area == val;
        }

        public List<GuestRequest> GetGuests(filter filter, int val)  //פונקציה שמחזירה רשימה של לקוחות לפי סינון של אחד מהאינם
        {
            return dal.ReadAllGuestRequest().Where(g => filter(g, val)).ToList();
        }
        
        public List<HostingUnit> MostWantedHostingUnit() //מחזיר את מס היחידה או היחידות עם הכי הרבה הזמנות
        {
            var allOrders = dal.ReadAllOrder();
            var orders = allOrders.GroupBy(x => x.HostingUnitKey, (key, g) => new { HostingUnitKey = key, oreders = g.Count() });
            int max = orders.Max(a => a.oreders);

            List<int> dd1 = orders.Where(h => h.oreders == max).ToList().Select(o => o.HostingUnitKey).ToList();
            List<HostingUnit> yy = new List<HostingUnit>();
            List<HostingUnit> yy1 = new List<HostingUnit>();
            foreach (var item in dd1)
            {
                yy= ReadAllHostingUnit().Where(t => t.HostingUnitKey == item).ToList();
                yy1.Add( ReadHostingUnit(item));
            }
            return yy;
        }

        //פונקציה שמחזירה את היחידות שנמצאות באיזור בארץ
        public delegate bool filterHostingArea(HostingUnit gu, Enums.Area area);
        public static bool HostingByArea(HostingUnit Hu, Enums.Area t) //פונקציה שמחזירה את כל היחידות בירושלים
        {
            return Hu.area == t;
        }

        public List<HostingUnit> AllHostingInJerusalem()
        {
            List<HostingUnit> temp=dal.ReadAllHostingUnit();
            List<HostingUnit> Jerusalem = new List<HostingUnit>();
            foreach (var item in temp)
            {
                if (HostingByArea(item, Enums.Area.Jerusalem)) 
                Jerusalem.Add(item);
            }
            return Jerusalem;
        }

        public List<HostingUnit> AllHostingInCenter() //פונקציה שמחזירה את כל היחידות במרכז
        {
            List<HostingUnit> temp = dal.ReadAllHostingUnit();
            List<HostingUnit> Center = new List<HostingUnit>();
            foreach (var item in temp)
            {
                if (HostingByArea(item, Enums.Area.Center))
                    Center.Add(item);
            }
            return Center;
        }

        public List<HostingUnit> AllHostingInNorth() //פונקציה שמחזירה את כל היחידות הצפון
        {
            List<HostingUnit> temp = dal.ReadAllHostingUnit();
            List<HostingUnit> North = new List<HostingUnit>();
            foreach (var item in temp)
            {
                if (HostingByArea(item, Enums.Area.North))
                    North.Add(item);
            }
            return North;
        }

        public List<HostingUnit> AllHostingInSouth() //פונקציה שמחזירה את כל היחידות בדרום
        {
            List<HostingUnit> temp = dal.ReadAllHostingUnit();
            List<HostingUnit> South = new List<HostingUnit>();
            foreach (var item in temp)
            {
                if (HostingByArea(item, Enums.Area.South))
                    South.Add(item);
            }
            return South;
        }

        bool checkDate(DateTime EntryDate, DateTime ReleaseDate) //תאריך תחילת הנופש קודם לפחות ביום אחד לתאריך סיום הנופש
        {
            if (EntryDate >= ReleaseDate)
            {
                return false;
            }
            return true;
        }

        bool checkStatus(BE.Order o) //פונקציה שבודקת אם הסטטוס השתנה לסגירת עסקה
        {
            if (o.orderStatus == Enums.OrderStatus.סגירת_עיסקה)
            {
                return true;
            }
            return false;
        }

        bool checkStatusMaile(BE.Order o) //פונקציה שבודקת אם הסטטוס השתנה לסגירת מייל
        {
            if (o.orderStatus == Enums.OrderStatus.נשלח_מייל)
            {
                return true;
            }
            return false;
        }

        int commission(BE.Order o) //פונקציה שמחשבת עמלה
        {
            int count = 0;
            dal = DAL.FactoryDal.GetIdal();
            GuestRequest guest = ReadGuestRequest(o.GuestRequestKey);
          
                    for (DateTime temp = guest.EntryDate; temp <= guest.ReleaseDate;temp= temp.AddDays(1))
                        count = count+1;

            return count * BE.Configuration.Commission;
        }

        void guestStatus(BE.Order o, Enums.OrderStatus val) //פונקציה שמשנה את הסטטוס של דרישת לקוח
        { 
            o.orderStatus = val;
            dal.UpdateOrder(o);
        }

        public List<GuestRequest> groupArea(Enums.Area area) //רשימת דרישות לקוח על פי אזור הנופש הנדרש
        {
            var vv = dal.ReadAllGuestRequest().GroupBy(a => a.area, (key, g) => new { area = key, guests = g.ToList() }).ToList();
            if ((int)area < 1)
            {
                throw new ZimmerException("The area is malfunctioning");
            }
            List<GuestRequest> fff = vv[(int)area].guests;
            return fff;
        }

        public List<GuestRequest> groupGuests(int numberOfGuests) //רשימת דרישות לקוח מקובצת על פי מספר הנופשים
        {
            var vv = dal.ReadAllGuestRequest().GroupBy(a => a.numberOfGuest, (key, g) => new { numberOfGuests = key, guests = g.ToList() }).ToList();
            List<GuestRequest> fff = vv[numberOfGuests].guests;
            if (fff != null)
                return fff;
            else throw new ZimmerException("The number of people mentioned above does not exist");
        }

        public List<HostingUnit> groupHostingUnit(Enums.Area area) //רשימת מארחים מקובצת על פי יחידות אירוח שהם מחזיקים
        {
            var vv = dal.ReadAllHostingUnit().GroupBy(a => a.area, (key, g) => new { area = key, guests = g.ToList() }).ToList();
            if ((int)area < 1)
            {
                throw new ZimmerException("The area is malfunctioning");
            }
            List<HostingUnit> fff = vv[(int)area].guests;
            return fff;
        }

        public void groupHostsByUnitCount() //עושה קבוצה לפי מספר יחידות
        {
            dal = DAL.FactoryDal.GetIdal();
            var orders = dal.ReadAllHostingUnit().GroupBy(u => u.Owner.HostKey, (key, g) => new { hostid = key, count = g.Count() }).GroupBy(c => c.count, (key, g) => new { count = key, owners = g.ToList() });
        }

        bool Search(Order t) //
        {
            if ((dal.ReadAllGuestRequest().Where(x => x.GuestRequestKey == t.GuestRequestKey).ToList() != null) && (dal.ReadAllHostingUnit().Where(x => x.HostingUnitKey == t.HostingUnitKey).ToList() != null))
                return true;
            else
                throw new ZimmerException("An order cannot be placed because the customer or unit does not exist");
        }

        bool checkDays(Order order)
        {
            dal = DAL.FactoryDal.GetIdal();
            GuestRequest guestRequest = dal.ReadGuestRequest(order.GuestRequestKey);
            HostingUnit hostingUnit = dal.ReadHostingUnit(order.HostingUnitKey);
            DateTime Temp = guestRequest.EntryDate;
            int numberOfDays = (guestRequest.ReleaseDate - guestRequest.EntryDate).Days;
            for (int i = 0; i < numberOfDays; i++)
            {
                if (hostingUnit.Diary[Temp.Month - 1, Temp.Day - 1] == true)
                {
                    throw new ZimmerException("Accommodation is not available");
                }
                else
                {
                    Temp.AddDays(1);
                }
            }
            return true;
        }

       public void ClosingConfirmation(Order order) //סגירת תאריכים
        {
            dal = DAL.FactoryDal.GetIdal();
            HostingUnit hostingUnit = new HostingUnit();
            if (order.orderStatus != dal.ReadOrder(order.OrderKey).orderStatus && order.orderStatus == Enums.OrderStatus.סגירת_עיסקה)
            {
                GuestRequest guestRequest = dal.ReadGuestRequest(order.GuestRequestKey);
                 hostingUnit = dal.ReadHostingUnit(order.HostingUnitKey);
                DateTime Temp = guestRequest.EntryDate;

                int numberOfDays = (guestRequest.ReleaseDate - guestRequest.EntryDate).Days;
                for (int i = 0; i < numberOfDays; i++)
                {
                    hostingUnit.Diary[Temp.Month - 1, Temp.Day - 1] = true;
                }
            }
            hostingUnit.Commission += commission(order);
            dal.UpdateHostingUnit(hostingUnit);
       
        }

        public int dates(DateTime one, DateTime two) //פונקציה שמקבלת שני תאריכים ומחזירה את מספר הימים בין התאריך הראשון לשני
        {
            int totalDays = (two - one).Days;
            return totalDays;
        }

        public int dates(DateTime one) //פונקציה שמקבלת תאריך ומחזירה את מספר הימים עד היום
        {
            int totalDays = (DateTime.Now - one).Days;
            return totalDays;
        }

        public List<Order> dealWasClosed(HostingUnit hostingUnit)//כמה עסקאות המארח סגר
        {
            List<Order> temp = dal.ReadAllOrder().Where(o => o.HostingUnitKey == hostingUnit.HostingUnitKey).ToList();
                List<Order> orders = temp.Where(f => f.orderStatus == Enums.OrderStatus.סגירת_עיסקה).ToList();
                return orders;
        }

        public List<Order> InvitationSent(HostingUnit hostingUnit)//רשימה של ההזמנות שנשלחו
        {
            List<Order> orders = dal.ReadAllOrder().Where(o => o.HostingUnitKey == hostingUnit.HostingUnitKey).ToList();
           
            return orders;
        }

        public void ChangesStatusAllCustomerOrders(Order t) // פונקציה שמשנה את הסטטוס של כל ההזמנות
        {
            List<Order> orders = ReadAllOrder().Where(x => x.GuestRequestKey == t.GuestRequestKey).ToList();
            foreach (var item in orders)
            {
                guestStatus(item, Enums.OrderStatus.נסגר_מחוסר_הענות_הלקוח);
            }

        }
    }
}
