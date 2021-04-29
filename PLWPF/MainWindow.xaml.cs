using BE;
using BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // private User loggedinUser = null;
        private bool isNewUser;

        
        LoginWindow loginWindow ;
        newHost newHostWindow;// = new newHost();
        HostingUnit hostingUnit;
        Administrator administrator;
  
        //קונסטרקטור
        public MainWindow()
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.FlowDirection = FlowDirection.RightToLeft;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            SystemCommands.MaximizeWindow(this);
            DataContext = this;
        }
        //אתחול רשימת הזמנות של לקוח שנשלח אליו מייל
        public List<Order> MyOrder
        {
            get
            {
                int refid = BL.BL_imp.loggedInUser.RefId;
                List<Order> orders = bl.ReadAllOrder().Where(x => x.GuestRequestKey == refid && x.orderStatus == BE.Enums.OrderStatus.נשלח_מייל).ToList();
                return orders;
            }
        }
        
        public GuestRequest myuestRequest
        {
            get
            {            
                return bl.ReadAllGuestRequest().Where(x => x.MailAddress == BL.BL_imp.loggedInUser.Email).FirstOrDefault();      
            }
        }
        //סגירה/שינוי גודל/להוריד למטה
        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e) //סגירת חלון
        {
            Environment.Exit(0);
        }

        private void Button_Click_MinimizeWindow(object sender, RoutedEventArgs e) //הורדת חלון
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void Button_Click_MaximizeWindow(object sender, RoutedEventArgs e) //הגדלת חלון
        {
            if (this.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(this);
            else
                SystemCommands.MaximizeWindow(this);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) 
        {
            ((Button)sender).Width *= 1.1;
            ((Button)sender).Height *= 1.1;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) 
        {
            ((Button)sender).Width /= 1.1;
            ((Button)sender).Height /= 1.1;
        }
        //פונקציה שפותחת חלון לפני איזה משתנה שהתחבר
        private void ShowLogin() 
        {
            loginWindow = new LoginWindow();
            BL_imp.loggedInUser = null;
            if (loginWindow.ShowDialog().Value)
            {
                userName.Content = BL_imp.loggedInUser.Username;
                newGuestRequest.Visibility = Visibility.Hidden;
                guestRequest.Visibility = Visibility.Hidden;
                OutPanel.Visibility = Visibility.Visible;
                isNewUser = loginWindow.isNewUser;

                switch (BL.BL_imp.loggedInUser.UserType)
                {                   
                    case UserTypes.Host:
                        if (isNewUser)
                        {
                            try
                            {
                                newHostWindow = new newHost();
                                newHostWindow.ShowDialog();
                            }
                            catch (BE.ZimmerException a)
                            {
                               
                                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        try
                        {
                            hostingUnit = new HostingUnit();
                            hostingUnit.ShowDialog();
                        }
                        catch (BE.ZimmerException a)
                        {
                            MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception a)
                        {
                            MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case UserTypes.Guest:
                        if (isNewUser)
                        {
                            newGuestRequest.Visibility = Visibility.Visible;
                            newGuestRequest.DataContext = new BE.GuestRequest();
                            UnitDate.IsEnabled = true;
                            guestRequest.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            newGuestRequest.Visibility = Visibility.Hidden;
                            guestRequest.Visibility = Visibility.Visible;
                        }
                        break;
                    case UserTypes.administrator:
                        administrator = new Administrator();
                        administrator.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
            else
                Environment.Exit(0);
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowLogin();
            MainArea.ItemsSource = Enum.GetValues(typeof(Enums.Area)).Cast<Enums.Area>();
            ResortTypes.ItemsSource = Enum.GetValues(typeof(Enums.ResortType)).Cast<Enums.ResortType>();
        }

        private void logOut(object sender, RoutedEventArgs e)
        {
            logOut();
        }

        private void logOut()
        {
            newGuestRequest.Visibility = Visibility.Hidden;
            guestRequest.Visibility = Visibility.Hidden;
            
            OutPanel.Visibility = Visibility.Hidden;
            BL_imp.loggedInUser = null;
            ShowLogin();
        }

        private BL.BL_imp bl = BL.FactoryBl.GetIBL();

        private void OrdersConfirmation1(object sender, RoutedEventArgs e)
        {     
                SelectedDatesCollection selectedDates = UnitDate.SelectedDates;
                if (selectedDates.Count == 0)
                {
                    MessageBox.Show("יש להכניס תאריכים", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (lineFirstName == null && lineFamilyName == null)
                {
                    MessageBox.Show("שדה זה הינו חובה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (lineFamilyName == null)
                    
                        //asterisk1.Visibility = Visibility.Visible;
                    if (lineFirstName == null)
                        //asterisk.Visibility = Visibility.Visible;
                    return;
                }


            BE.GuestRequest guest = (BE.GuestRequest)((Button)sender).DataContext;
            guest.EntryDate = UnitDate.SelectedDate.Value;
            guest.ReleaseDate = UnitDate.SelectedDates.Last();

            if (BL_imp.loggedInUser.RefId == 0)
            {
                try
                {
                    guest.MailAddress = BL_imp.loggedInUser.Email;
                    BL_imp.loggedInUser.RefId = (bl.CreateGuestRequest(guest));
                    bl.updateUser(BL_imp.loggedInUser.Id, BL_imp.loggedInUser.RefId);
                    int IdGuest = BL_imp.loggedInUser.RefId;
                    createOrders(IdGuest);
                }
                catch (BE.ZimmerException a)
                {

                    MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }      
            
            else
            {
                //GuestRequest guest1 = bl.ReadGuestRequest(BL_imp.loggedInUser.RefId);
                GuestRequest  guest1 = (BE.GuestRequest)((Button)sender).DataContext;
                try
                {
                    bl.UpdateGuestRequest(guest1);
                }
                catch (BE.ZimmerException a)
                {

                    MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            newGuestRequest.Visibility = Visibility.Hidden;
            guestRequest.Visibility = Visibility.Visible;
        }
        // יצירת הזמנה
        private void createOrders(int idGuest)
        {
            try
            {
                List<BE.HostingUnit> listHostingUnit = bl.ReadAllHostingUnit().ToList();
                BE.GuestRequest guest = bl.ReadAllGuestRequest().Where(x => x.GuestRequestKey == idGuest).FirstOrDefault();
                foreach (var item in listHostingUnit)
                {
                    if (check(item, guest))
                    {
                        Order order = new Order();
                        order.GuestRequestKey = guest.GuestRequestKey;
                        order.HostingUnitKey = item.HostingUnitKey;
                        order.orderStatus = BE.Enums.OrderStatus.טרם_טופל;
                        order.OrderDate = DateTime.Now;

                        bl.CreateOrder(order);
                    }
                }
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //סגירת עיסקה
        private bool check(BE.HostingUnit item, GuestRequest guest)
        {
            if((item.area== guest.area) &&(item.subArea==guest.subArea) &&(Equal_(item.childrensAttractions, guest.childrensAttractions)
                &&(Equal_(item.DisabledAccessible,guest.DisabledAccessible))&&(Equal_(item.WindowToTheSea,guest.WindowToSea))&&(Equal_(item.ThereIsPool , guest.pool))
                &&(Equal_(item.jacuzzi, guest.jacuzzi))))
            {

                return true;
            }
            return false;
        }

        private bool Equal_(Enums.Yes_No unit, Enums.enum_1 guest)
        {
            if (guest == Enums.enum_1.לא_מעוניין && unit == Enums.Yes_No.Yes)
                return false;
            if (guest == Enums.enum_1.הכרחי&& unit == Enums.Yes_No.No)
                return false;
            return true;
        }

          

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BL_imp.loggedInUser = null;
            BL_imp.loggedInHost = null;
            OutPanel.Visibility = Visibility.Hidden;
            newGuestRequest.Visibility = Visibility.Hidden;
            guestRequest.Visibility = Visibility.Hidden;
            GuestOrders.Visibility = Visibility.Hidden;
            ShowLogin();
        }

        private void Area(object sender, RoutedEventArgs e)
        {

        }
        //מחיקת לקוח
        private void del_GuestRequest_click(object sender, RoutedEventArgs e)

        {
            GuestRequest MyguestRequest = new GuestRequest();
            try
            {
                 MyguestRequest = bl.ReadAllGuestRequest().Where(x => x.GuestRequestKey == BL_imp.loggedInUser.RefId).FirstOrDefault();
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            guestRequest.Visibility = Visibility.Hidden;
            MessageBoxResult yesNo = MessageBox.Show("האם למחוק את הבקשה?", " ", MessageBoxButton.YesNo);
            if (yesNo == MessageBoxResult.Yes)
            {
                try
                {
                    bl.DeleteGuestRequest(MyguestRequest.GuestRequestKey);
                    bl.DeleteUser(BL_imp.loggedInUser.Id);
                }
                catch (ZimmerException a)
                {
                    MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MessageBox.Show("בקשתך נמחקה בהצלחה", " ", MessageBoxButton.OK);
                logOut();
            }
            else
            {
                guestRequest.Visibility = Visibility.Visible;
            }
         

           
        }

        private void OrderMyGuest_click(object sender, RoutedEventArgs e)
        {
            newGuestRequest.Visibility = Visibility.Hidden;
            guestRequest.Visibility = Visibility.Hidden;

            GuestOrders.ItemsSource = null;
            GuestOrders.ItemsSource = MyOrder;
            GuestOrders.Visibility = Visibility.Visible;

        }
        //עדכון לקוח
        private void UpdateMyRequest_click(object sender, RoutedEventArgs e)
        {
            GuestRequest gr = new GuestRequest();
            newGuestRequest.Visibility = Visibility.Visible;
            try
            {
                gr = bl.ReadGuestRequest(BL.BL_imp.loggedInUser.RefId);
                newGuestRequest.DataContext = gr;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            for (int i = 0; gr.EntryDate.AddDays(i) <= gr.ReleaseDate; i++)
            {
                UnitDate.SelectedDates.Add(gr.EntryDate.AddDays(i));
            }
            
            UnitDate.IsEnabled = false;

            guestRequest.Visibility = Visibility.Hidden;
        }

        private void MainArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSecArea();
        }

        private void SelectSecArea()
        {
            SecArea.ItemsSource = BE.Enums.areas[(Enums.Area) MainArea.SelectedItem];
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void updateRequest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResortTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //סגירת עיסקה
        private void Button_Click_Colsing_Deal(object sender, RoutedEventArgs e)
        {
            try
            {
                BE.Order order = (BE.Order)((Button)sender).DataContext;
                bl.ChangesStatusAllCustomerOrders(order);
                GuestRequest guestRequest = bl.ReadGuestRequest(order.GuestRequestKey);
                order.orderStatus = BE.Enums.OrderStatus.סגירת_עיסקה;
                guestRequest.status = BE.Enums.CustomerRequirementStatus.closed;
                bl.ClosingConfirmation(order);
                bl.UpdateOrder(order);
                bl.UpdateGuestRequest(guestRequest);
                GuestOrders.ItemsSource = null;
                GuestOrders.ItemsSource = MyOrder;
                
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Order order= (BE.Order)((DataGrid)sender).SelectedItem;
        }     
    }
}