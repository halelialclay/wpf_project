using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    interface IBL
    {
        //פונקציות כמו ב dal
        int CreateHostingUnit(HostingUnit t, Host host);
        HostingUnit ReadHostingUnit(int Id);
        List<HostingUnit> ReadAllHostingUnit();
        bool UpdateHostingUnit(HostingUnit t);
        bool DeleteHostingUnit(int Id);
        int CreateGuestRequest(GuestRequest t);
        GuestRequest ReadGuestRequest(int Id);
        List<GuestRequest> ReadAllGuestRequest();
        bool UpdateGuestRequest(GuestRequest guestRequest);
        bool DeleteGuestRequest(int Id);
        int CreateOrder(Order t);
        Order ReadOrder(int Id);
        List<Order> ReadAllOrder();
        bool UpdateOrder(Order t);
        bool DeleteOrder(int Id);
        List<BankAccount> GetListBankAccount();
        //פונקציות חדשות
        List<HostingUnit> AccommodationUnitsAvailable(DateTime date, int numberOfDays);//יחידות אירוח זמינות
        // פונקציות שמחזירות כמה זמן עבר בין 2 תאריכים
        int dates(DateTime one, DateTime two);
        int dates(DateTime one);
        //החזרת הזמנות
        List<Order> TheWaitingTimeExpired(int numberOfDays);
        //לא ידוע
         List<Order> dealWasClosed(HostingUnit hostingUnit);
        List<Order> InvitationSent(HostingUnit hostingUnit);
        //פונקציה שמקבלת לקוח ומחזירה כמה הזמנות יש לו
        int numberOfOrdersForGuest(GuestRequest g);
        int numberOfOrdersForHostingUnit(HostingUnit h);
        //פונקציות נוספות
        List<HostingUnit> UnitsInJerusalemWithPoolAndDisabledAccess();
        List<HostingUnit> MostWantedHostingUnit();
    }
}
   
