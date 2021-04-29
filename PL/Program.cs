using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BE;

namespace PL
{
    class Program
    {
        public enum user {begin, guestrequest=1, hostingunit, oeder, Webmaster,exsit }
        public enum UserAction { create=1, Read, Update, Delete, ReadAll }
        static void Main(string[] args)
        {
            BL.BL_imp bL = BL.FactoryBl.GetIBL();
            Console.WriteLine("Press any key to start.\n");
          
            var yore_choise = Console.ReadLine(); Console.WriteLine(" ");

            while (Convert.ToInt32(yore_choise) != 5)
            {
                try
                {
                    Console.WriteLine("Choose a number from one to four");
                    Console.Write("1- If you are a customer\n" + "2 -If you own a unit\n" + "3-For order\n" + "4-Webmaster\n"+ "5-Log off\n");
                    yore_choise = Console.ReadLine(); Console.WriteLine(" ");

                    switch (yore_choise)
                    {
                        case "0":   
                            break;

                        case "1": //"guestrequest":
                            {
                               Console.Write("1-If you want to create a hosting request\n" +
                                   "2 -If you want to see your order\n" +
                                   "3-If you want to change your order\n" +
                                   "4-If you want to delete your order\n"+
                                   "5-exite\n");
                                var choise2 = Console.ReadLine();

                                switch (choise2)
                                {
                                    case "1": //"create":
                                        BE.GuestRequest temp = new BE.GuestRequest();
                                        Console.WriteLine("what is your name?");
                                        var  name = Console.ReadLine();
                                        temp.PrivateName = name;
                                        Console.WriteLine("what is your last name?\n");
                                        var lastname = Console.ReadLine();
                                        temp.FamilyName= name;
                                        Console.WriteLine("Please enter an email address\n");
                                        var email = Console.ReadLine();
                                       name= emailGood(email);
                                        temp.MailAddress = name;
                                        //temp.RegistrationDate = DateTime.Now;
                                        Console.WriteLine("When do you want to start your vacation?\n");
                                        Console.WriteLine("day/month/yera\n");
                                        temp.EntryDate = ReadDate();

                                        Console.WriteLine("When do you want to end your vacation?\n");
                                        Console.WriteLine("day/month/yera\n");
                                        temp.ReleaseDate = ReadDate();

                                        Console.Write("Which area do you want?\n" + "1-Jerusalem/2-Center/3-North/4-South\n");
                                        temp.area = (BE.Enums.Area)ReadInt(1, 4);
                                   
                                        Console.WriteLine("How many adults?");
                                        temp.Adults= ReadInt(0, 100);

                                        Console.WriteLine("How many children?");
                                        temp.Children=  ReadInt(0, 100);
                                        temp.numberOfGuest = temp.Adults + temp.Children;
                                        Console.WriteLine("You want a pool");
                                        Console.WriteLine("1-Must, 2-preferably, 3-not interested");
                                        temp.pool = (BE.Enums.enum_1)ReadInt(1,3);
                                        Console.WriteLine("You want a Jacuzzi");
                                        Console.WriteLine("1-Must, 2-preferably, 3-not interested");
                                        temp.jacuzzi = (BE.Enums.enum_1)ReadInt(1, 3);
                                        Console.WriteLine("You want a Window To Sea");
                                        Console.WriteLine("1-Must, 2-preferably, 3-not interested");
                                        temp.WindowToSea =(BE.Enums.enum_1)ReadInt(1, 3);
                                        Console.WriteLine("You want a childrens Attractions");
                                        Console.WriteLine("1-Must, 2-preferably, 3-not interested");
                                        temp.childrensAttractions = (BE.Enums.enum_1)ReadInt(1, 3); 
                                        Console.WriteLine("You want a childrens garden");
                                        Console.WriteLine("1-Must, 2-preferably, 3-not interested");
                                        temp.garden = (BE.Enums.enum_1)ReadInt(1, 3);

                                        Console.Write("Your site access number:\n");
                                        Console.WriteLine(bL.CreateGuestRequest(temp));
                                        break;          
                                    case "2"://Read:
                                        Console.WriteLine("What is your site access number?");
                                        var id = Console.ReadLine();
                                        Console.WriteLine ( bL.ReadGuestRequest(Convert.ToInt32(id)).ToString());

                                        break;
                                    case "3"://change
                                        Console.WriteLine("What is your site access number?");
                                       GuestRequest guestRequestChange= bL.ReadGuestRequest(Convert.ToInt32(Console.ReadLine()));
                                        Console.WriteLine("What do you want to change?");
                                        Console.Write("\n");
                                        break;
                                    case "4"://Delete
                                        Console.WriteLine("What is your site access number?");
                                        var iddelete = Console.ReadLine();
                                        bL.DeleteGuestRequest(Convert.ToInt32(iddelete));

                                        break;

                                    default:
                                        Console.Write("ERORR\n");
                                        break;

                                }

                                break;
                            }

                        case "2"://hostingunit
                            {
                                Console.Write("1-If you want to create a hosting unit\n" +
                                    "2 -If you want to see your order\n" +
                                    "3-If you want to change your order\n" +
                                    "4-If you want to delete your order\n" +
                                    "5-exite\n");
                                var choise1 = Console.ReadLine();
                            

                                switch (choise1)
                                {
                                    case "1": //"create":
                                        HostingUnit temp = new HostingUnit();
                                        Console.WriteLine("Unit name?");
                                      temp.HostingUnitName= Console.ReadLine();
                                        Console.Write("Where is the accommodation unit?\n"+ "1-Center/2-Jerusalem/3-North/4-South");
                                        temp.area = (Enums.Area)ReadInt(1, 4);
                                        Console.Write("Is there a window to the sea?\n"+"1-Yes,0-no");
                                        temp.WindowToTheSea =  (Enums.Yes_No)ReadInt(0,1);
                                        Console.Write("Is there a pool?\n "+ "1-Yes,0-no");
                                        temp.ThereIsPool = (Enums.Yes_No)ReadInt(0,1);
                                        Console.Write("Is there a Disabled Accessible?\n " + "1-Yes,0-no");
                                        temp.DisabledAccessible = (Enums.Yes_No)ReadInt(0, 1);
                                        Console.Write("Is there a garden?\n " + "1-Yes,0-no");
                                       
                                        Console.Write("Is there a garden?\n " + "1-Yes,0-no");
                                        temp.childrensAttractions = (Enums.Yes_No)ReadInt(0, 1);
                                        Console.Write("Is there a jacuzzi?\n " + "1-Yes,0-no");
                                        temp.jacuzzi = (Enums.Yes_No)ReadInt(0, 1);
                                        Console.Write("\n"+ "1-Zimmer/2-guestRoom3-HotelRoom/4-encampment");
                                        temp.resortType = (Enums.ResortType)ReadInt(1, 4);
                                        temp.numberOfPlaces = ReadInt(1,50);

                                        Console.WriteLine("the number of id is: ");
                                      //  Console.WriteLine(   bL.CreateHostingUnit(temp));


       
       
                                        break;

                                    case "2"://Read:
                                        Console.WriteLine("What is your site access number?");
                                        var id = Console.ReadLine();
                                        Console.WriteLine(bL.ReadHostingUnit(Convert.ToInt32(id)).ToString());
                                        break;
                                    case "3"://change
                                    
                                            Console.WriteLine("What is your site access number?");
                                            var iddelete = Console.ReadLine();
                                            var temp1 = bL.ReadHostingUnit(Convert.ToInt32(iddelete));
                                            Console.WriteLine("What do you want to change?");
                                            Console.Write("1-Name of accommodation unit\n"
                                                + "2-The hosting occupancy\n" +
                                                "3-Details at the host\n");
                                            switch (ReadInt(1, 3))
                                            {
                                                case 1://name
                                                    Console.WriteLine("A new name is:");
                                                    temp1.HostingUnitName = Console.ReadLine();
                                                    break;
                                                case 2://daire
                                                    break;
                                                case 3://host
                                                    Console.Write("1-For approval to the bank \n" +
                                                        "2-email\n"+
                                                        "3-Fhone Number\n");
                                                    switch (ReadInt(1, 3))
                                                    {
                                                        case 1:

                                                           temp1.Owner.CollectionClearance = (BE.Enums.Yes_No)ReadInt(0, 1);
                                                            break;
                                                        case 2:
                                                        Console.WriteLine("enter a new email");
                                                        temp1.Owner.MailAddress=emailGood( Console.ReadLine());

                                                        break;
                                                         case 3:
                                                        Console.WriteLine("enter a new fhone");
                                                        temp1.Owner.PhoneNumber=ReadInt(0500000000,0580000000);
                                                          break;

                                                    }
                                                    break;
                                            }
                                        bL.UpdateHostingUnit(temp1);
                                        break;

                                    case "4"://delete
                                        Console.WriteLine("What is your site access number?");
                                         iddelete = Console.ReadLine();
                                        bL.ReadHostingUnit(Convert.ToInt32(iddelete));
                                        break;


                                }
                                break;
                            }

                        case "3"://order
                            {
                                Console.Write("1-If you want to create a order\n" +
                         "2 -If you want to see your order\n" +
                         "3-If you want to change status\n" +
                         "4-If you want to delete your order\n" +
                         "5-exite\n");
                                var choise = Console.ReadLine();


                                switch (choise)
                                {
                                    case "1": //"create":
                                        Order order = new Order();
                                        Console.WriteLine("enter a hosting request ID number:  ");
                                        order.GuestRequestKey = ReadInt(30000001,399999999);
                                        Console.WriteLine("enter a hosting unit ID number:  ");
                                        order.HostingUnitKey= ReadInt(20000001, 299999999);
                                        Console.WriteLine("The order number is:");
                                        Console.WriteLine(bL.CreateOrder(order));
                                        break;

                                    case "2"://Read:
                                        Console.WriteLine("What is your site access number?");
                                        var id = Console.ReadLine();
                                        Console.WriteLine(bL.ReadOrder(Convert.ToInt32(id)).ToString());
                                        break;
                                    case "3"://change
                                        Console.WriteLine("What is your site access number?");
                                        var iddelete = Console.ReadLine();
                                        var temp = bL.ReadOrder(Convert.ToInt32(iddelete));
                                        Console.WriteLine("order status:");
                                   
                                                Console.Write( "new Statos: \n"+
                                                    "1- open" + "2-closed" + "3-expired");
                                        temp.orderStatus = (Enums.OrderStatus)ReadInt(1, 3);

                                        bL.UpdateOrder(temp);

                                        break;

                                    case "4"://delete
                                        Console.WriteLine("What is your site access number?");
                                        iddelete = Console.ReadLine();
                                        bL.ReadOrder(Convert.ToInt32(iddelete));
                                        break;


                                }
                                break;
                            }

                        case "4"://Webmaster
                            {
                                Console.WriteLine("guest Requests Pool");
                                List<GuestRequest> guestRequestsPool= bL.AllGuestsWithPool();
                                printList(guestRequestsPool);
                                Console.WriteLine("Most Hosting");
                                //List < Int32 > MostHosting= bL.MostWantedHostingUnit();
                        //        printList(MostHosting);
                                List < HostingUnit > UnitsInJerusalemWithPoolAndDisabledAccess = bL.UnitsInJerusalemWithPoolAndDisabledAccess();
                                Console.WriteLine("Units In Jerusalem With Pool And Disabled Access ");
                                printList(UnitsInJerusalemWithPoolAndDisabledAccess);
                                List<Order> TheWaitingTimeExpired = bL.TheWaitingTimeExpired(BE.Configuration.SeveralDaysToExpireOrder);
                                Console.WriteLine("The Waiting Time Expired");
                                printList(TheWaitingTimeExpired);
                                Console.WriteLine("enter two day : day/month/year");
                                DateTime one = ReadDate(), two = ReadDate();
                                Console.WriteLine( bL. dates(one, two));
                                Console.WriteLine( bL.dates(one));
                                bL.groupHostsByUnitCount();
                                Console.WriteLine("enter number of HostingUnit 200000001-299999999");
                                List<Order> hostOrder = bL.dealWasClosed(bL.ReadHostingUnit(ReadInt(200000001,299999999)));
                                printList(hostOrder);
                                Console.WriteLine("enter number of HostingUnit 200000001-299999999");
                                List<Order> Invitation_Sent = bL.InvitationSent(bL.ReadHostingUnit(ReadInt(200000001, 299999999)));
                                printList(Invitation_Sent);
                                Console.WriteLine("enter number of GuestRequest 100000001-199999999");
                                   int numberOfOrdersForGuest=bL.numberOfOrdersForGuest(bL.ReadGuestRequest(ReadInt(100000001,199999999)));
                                Console.WriteLine("enter number of HostingUnit 200000001-299999999");
                                int numberOfOrdersForHostingUnit= bL.numberOfOrdersForHostingUnit(bL.ReadHostingUnit(ReadInt(200000001, 299999999)));
                                List<GuestRequest> guestRequestsJerusalem = bL. groupArea(Enums.Area.Jerusalem);
                                Console.WriteLine("enter number of GuestRequest 100000001-199999999");
                                List<GuestRequest> groupGuests = bL.groupGuests(ReadInt(100000001, 199999999));
                                List<HostingUnit> groupHostingUnit = bL.groupHostingUnit(Enums.Area.Jerusalem);


                                break;
                            }
                        default:
                            Console.Write("ERORR\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        private static int ReadInt(int minval, int maxval)
        {
            int result = 0;
            string number = Console.ReadLine();

            while (!Int32.TryParse(number, out result) && (result < minval || result > maxval))
            {
                Console.Write("Not a valid date, try again.\n");
                number = Console.ReadLine();
            }


            return result;

        }

        private static DateTime ReadDate()
        {
            DateTime dateResult = DateTime.MinValue;
            string date = Console.ReadLine();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("he-IL");

            while (!DateTime.TryParse(date, culture, DateTimeStyles.None, out dateResult))
            {
                Console.Write("Not a valid date, try again.\n");
                date = Console.ReadLine();
            }

            return dateResult;

        }


        public static string emailGood(string email)
        {
            
            while(!( email.IndexOf('@') < email.IndexOf('.')))
            {

                    Console.WriteLine("enter new email");
                    email = Console.ReadLine();

            }
            return email;


        }
        public static void printList<T>(List <T> ts )
        {
            if(ts!=null)
            foreach (var item in ts)
            {
                Console.Write(item.ToString());
                    Console.WriteLine("\n");

            }
            else
                Console.WriteLine("Not found");
        }

     
    }
}