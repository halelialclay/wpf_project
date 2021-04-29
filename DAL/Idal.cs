using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DAL
{ 
    interface Idal
    {
        int CreateHostingUnit(HostingUnit t,Host host);
        HostingUnit ReadHostingUnit(int Id);
        List<HostingUnit> ReadAllHostingUnit();
        bool UpdateHostingUnit(HostingUnit t);
        bool DeleteHostingUnit(int Id);
        int CreateGuestRequest(GuestRequest t);
        GuestRequest ReadGuestRequest(int Id);
        List<GuestRequest> ReadAllGuestRequest();
        bool UpdateGuestRequest( GuestRequest guestRequest);
        bool DeleteGuestRequest(int Id);
        int CreateOrder(Order t);
        Order ReadOrder(int Id);
        List<Order> ReadAllOrder();
        bool UpdateOrder(Order order);
        bool DeleteOrder(int Id);
        List<BankAccount>GetListBankAccount();
        bool register(User ruser);
        bool DeleteUser(int Id);
        User GetUser(string lUserName, string lPassword);
    }
}
