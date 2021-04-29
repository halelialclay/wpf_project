using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public static class DataSource //רשימות של דרישות לקוח, יחידות אירוח ובנקים
    {
        public static List<GuestRequest> guestrequest = new List<GuestRequest>()
        {
            new GuestRequest()
            {
               GuestRequestKey=300000002,
               PrivateName= "Shir",
               FamilyName= "Mass",
               MailAddress="massshir@gmail.com",
               status= Enums.CustomerRequirementStatus.open,
               RegistrationDate=new DateTime(2019,10,2),
               EntryDate=new DateTime(2020,4,3),
               ReleaseDate=new DateTime(2020,4,8),
               area= Enums.Area.Center,
               subArea= "תל-אביב",
               type= Enums.ResortType.HotelRoom,
               Adults=2,
               Children=4,
               pool= Enums.enum_1.הכרחי,
               garden= Enums.enum_1.לא_מעוניין,
               childrensAttractions= Enums.enum_1.הכרחי,
               jacuzzi= Enums.enum_1.אפשרי,
               numberOfGuest=6
            },

            new GuestRequest()
            {
               GuestRequestKey= 300000003,
               PrivateName= "Carmit",
               FamilyName= "Rubinez",
               MailAddress="carmitrubinez@gmail.com",
               status= Enums.CustomerRequirementStatus.closed,
               RegistrationDate=new DateTime(2020,1,5),
               EntryDate=new DateTime(2020,5,9),
               ReleaseDate=new DateTime(2020,5,14),
               area= Enums.Area.North,
               subArea="טבריה",
               type= Enums.ResortType.Zimmer,
               Adults=2,
               Children=3,
               pool= Enums.enum_1.הכרחי,
               garden= Enums.enum_1.הכרחי,
               childrensAttractions= Enums.enum_1.הכרחי,
               jacuzzi= Enums.enum_1.לא_מעוניין,
               numberOfGuest=5
            },

            new GuestRequest()
            {
               GuestRequestKey= 300000004,
               PrivateName= "Lital",
               FamilyName= "Alclay",
               MailAddress="liel1976@gmail.com",
               status= Enums.CustomerRequirementStatus.open,
               RegistrationDate=new DateTime(2020,4,3),
               EntryDate=new DateTime(2020,6,8),
               ReleaseDate=new DateTime(2020,8,13),
               area= Enums.Area.South,
               subArea="אילת",
               type= Enums.ResortType.guestRoom,
               Adults=2,
               Children=0,
               pool= Enums.enum_1.אפשרי,
               garden= Enums.enum_1.אפשרי,
               childrensAttractions= Enums.enum_1.לא_מעוניין,
               jacuzzi= Enums.enum_1.הכרחי,
               numberOfGuest=2
            }
        };

        public static List<HostingUnit> hostingunit = new List<HostingUnit>()
        {
            new HostingUnit()
            {
                HostingUnitKey= 200000002,
                Owner= new Host(){HostKey=1, PrivateName="Noam", FamilyName="Avraham", MailAddress="massshir@gmail.com" },
                HostingUnitName= "מול הים",
                WindowToTheSea= Enums.Yes_No.Yes,
                ThereIsPool= Enums.Yes_No.No,
                DisabledAccessible= Enums.Yes_No.No,
                childrensAttractions=  Enums.Yes_No.No,
                jacuzzi=  Enums.Yes_No.No,
                area= Enums.Area.South,
                resortType= Enums.ResortType.HotelRoom,
                numberOfPlaces=6
            },

            new HostingUnit()
            {
                HostingUnitKey= 200000003,
                Owner= new Host(){HostKey=1,PrivateName="Noam", FamilyName="Avraham", MailAddress="massshir@gmail.com" },
                HostingUnitName= "בקתה בכפר",
                WindowToTheSea= Enums.Yes_No.No,
                ThereIsPool= Enums.Yes_No.Yes,
                DisabledAccessible= Enums.Yes_No.Yes,
                childrensAttractions=  Enums.Yes_No.No,
                jacuzzi=  Enums.Yes_No.Yes,
                area= Enums.Area.Center,
                resortType= Enums.ResortType.Zimmer,
                numberOfPlaces=5
            },

           new HostingUnit()
            {
                HostingUnitKey= 200000004,
                Owner= new Host(){HostKey=3,PrivateName="Moran", FamilyName="Cohen", MailAddress="massshir@gmail.com" },
                HostingUnitName= "מול החוף",
                WindowToTheSea= Enums.Yes_No.Yes,
                ThereIsPool= Enums.Yes_No.No,
                DisabledAccessible= Enums.Yes_No.No,
                childrensAttractions=  Enums.Yes_No.No,
                jacuzzi=  Enums.Yes_No.Yes,
                area= Enums.Area.North,
                resortType= Enums.ResortType.HotelRoom,
                numberOfPlaces=6
            },

            new HostingUnit()
            {
                HostingUnitKey= 200000005,
                Owner= new Host(){HostKey=3, PrivateName="Moran", FamilyName="Cohen", MailAddress="massshir@gmail.com" },
                HostingUnitName= "בקתה",
                WindowToTheSea= Enums.Yes_No.No,
                ThereIsPool= Enums.Yes_No.Yes,
                DisabledAccessible= Enums.Yes_No.Yes,
                childrensAttractions=  Enums.Yes_No.Yes,
                jacuzzi=  Enums.Yes_No.No,
                area= Enums.Area.Jerusalem,
                resortType= Enums.ResortType.Zimmer,
                numberOfPlaces=5
            },

            new HostingUnit()
            {
                HostingUnitKey= 200000006,
                Owner= new Host(){HostKey=2, PrivateName="Or", FamilyName="Levi", MailAddress="massshir@gmail.com" },
                HostingUnitName= "דירה קסומה",
                WindowToTheSea= Enums.Yes_No.Yes,
                ThereIsPool= Enums.Yes_No.No,
                DisabledAccessible= Enums.Yes_No.No,
                childrensAttractions=  Enums.Yes_No.No,
                jacuzzi=  Enums.Yes_No.Yes,
                area= Enums.Area.Jerusalem,
                resortType= Enums.ResortType.guestRoom,
                numberOfPlaces=2
            }
        };

        public static List<Order> order = new List<Order>()
        {
            new Order()
            {
                HostingUnitKey=200000002,
                GuestRequestKey=300000002,
                OrderKey=1000000,
                CreateDate=new DateTime(2020,3,20),
                OrderDate=new DateTime(2020,3,28),
            },

            new Order()
            {
                HostingUnitKey=200000003,
                GuestRequestKey=300000003,
                OrderKey=1000001,
                CreateDate=new DateTime(2020,4,29),
                OrderDate=new DateTime(2020,5,1),
            },

            new Order()
            {
                HostingUnitKey=200000006,
                GuestRequestKey=300000004,
                OrderKey=1000002,
                CreateDate=new DateTime(2020,7,5),
                OrderDate=new DateTime(2020,7,15),
                  orderStatus=Enums.OrderStatus.טרם_טופל,
            },

            new Order()
            {
                HostingUnitKey=200000005,
                GuestRequestKey=300000004,
                OrderKey=1000003,
                CreateDate=new DateTime(2020,7,5),
                OrderDate=new DateTime(2020,7,15),
                orderStatus=Enums.OrderStatus.טרם_טופל,
            },

            new Order()
            {
                HostingUnitKey=200000003,
                GuestRequestKey=300000004,
                OrderKey=1000004,
                CreateDate=new DateTime(2020,7,5),
                OrderDate=new DateTime(2020,7,15),
                orderStatus=Enums.OrderStatus.טרם_טופל,
            },

            new Order()
            {
                HostingUnitKey=200000003,
                GuestRequestKey=300000004,
                OrderKey=1000005,
                CreateDate=new DateTime(2020,7,5),
                OrderDate=new DateTime(2020,7,15),
                orderStatus=Enums.OrderStatus.טרם_טופל,
            },

            new Order()
            {
                HostingUnitKey=200000004,
                GuestRequestKey=300000004,
                OrderKey=1000006,
                CreateDate=new DateTime(2020,7,5),
                OrderDate=new DateTime(2020,7,15),
                orderStatus=Enums.OrderStatus.טרם_טופל,
            },

            new Order()
            {
                HostingUnitKey=200000004,
                GuestRequestKey=300000004,
                OrderKey=1000007,
                CreateDate=new DateTime(2020,7,5),
                OrderDate=new DateTime(2020,7,15),
            }
        };

        public static List<BankAccount> banks = new List<BankAccount>()
        {
            new BankAccount()
            {
                BankNumber=122,
                BankName ="הפועלים",
                BranchNumber = 35,
                BranchAddress = "הרצל 7",
                BranchCity = "קרית אונו",
                //BankAccountNumber = 856742
            },

            new BankAccount()
            {
                 BankNumber=456,
                BankName ="לאומי",
                BranchNumber = 76,
                BranchAddress = "כנפי נשרים 10",
                BranchCity = "ירושלים",
                //BankAccountNumber = 492764
            },

            new BankAccount()
            {
                BankNumber=390,
                BankName ="דיסקונט",
                BranchNumber = 24,
                BranchAddress = "יגאל אלון 5",
                BranchCity = "פתח תקווה",
                //BankAccountNumber = 948327
            },

            new BankAccount()
            {
                BankNumber=650,
                BankName ="איגוד",
                BranchNumber = 59,
                BranchAddress = "ביאליק 20",
                BranchCity = "תל אביב",
                //BankAccountNumber = 375648
            },

            new BankAccount()
            {
                BankNumber=720,
                BankName ="הבינלאומי",
                BranchNumber = 22,
                BranchAddress = "בן גוריון 18",
                BranchCity = "הרצליה",
                //BankAccountNumber = 564387
            }
        };

        public static List<User> users = new List<User>()
        {
            new User(){
                Id=1,
                RefId=200000003,
                Username="user1",
                Password="user1",
                UserType=UserTypes.Host
            },

            new User(){
                Id=2,
                RefId=200000006,
                Username="user2",
                Password="user2",
                UserType=UserTypes.Host
            },
            new User(){
                Id=3,
                RefId=200000004,
                Username="user3",
                Password="user3",
                UserType=UserTypes.Host
            },

            new User(){
                Id=4,
                RefId=300000002,
                Username="guest1",
                Password="guest1",
                UserType=UserTypes.Guest,
            },

            new User(){
                Id=5,
                 RefId=300000003,
                Username="guest2",
                Password="guest2",
                UserType=UserTypes.Guest
            },

            new User(){
                Id=6,
                RefId=300000004,
                Username="guest3",
                Password="guest3",
                UserType=UserTypes.Guest
            },

            new User()
            {
                Id=999999999,
                RefId=999999999,
                Username="admin",
                Password="admin",
                UserType=UserTypes.administrator
            }
        };
    }
}
