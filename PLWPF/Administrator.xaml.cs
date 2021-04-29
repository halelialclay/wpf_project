using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;
namespace PLWPF
{

    /// <summary>
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        private BL.BL_imp bl = BL.FactoryBl.GetIBL();

        public List<BE.GuestRequest> MyGuest
        {
            get
            {
                return bl.ReadAllGuestRequest();
            }
        }

        public List<BE.HostingUnit> MyHost
        {
            get
            {
                return bl.ReadAllHostingUnit();
            }
        }

        private List<BE.HostingUnit> _HostingUnitName= new List<BE.HostingUnit>();
        public List<BE.HostingUnit> HostingUnitName
        { 
            get
            {
                return _HostingUnitName;
            }
        }

        private List<BE.GuestRequest> _GuestRequestPool = new List<BE.GuestRequest>();
        public List<BE.GuestRequest> GuestRequestPool
        {
            get
            {
                return _GuestRequestPool;
            }
        }

        public Administrator()
        {
            InitializeComponent();
            listBox_guest.ItemsSource = MyGuest;
            listBox_HostingUnit.ItemsSource = MyHost;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.GuestRequest item = (BE.GuestRequest)listBox_guest.SelectedItem;        
            GuestDetails.Text = item.ToString();
        }

        private void ListBox_HostingUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.HostingUnit item = (BE.HostingUnit)listBox_HostingUnit.SelectedItem;
            DockPanel panel = new DockPanel();
            HostinUnitDetails.Text = item.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = (BL.BL_imp.loggedInUser!=null);
        }


        private void Hosting1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _HostingUnitName = bl.UnitsInJerusalemWithPoolAndDisabledAccess();
                listBox_HostingUnit_select.ItemsSource = HostingUnitName;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GuestRequest_Click(object sender, RoutedEventArgs e)
        {
            _GuestRequestPool = new List<BE.GuestRequest>();
            foreach (var item in bl.ReadAllGuestRequest())
            {
                if(BL_imp.guestPool(item, (int)BE.Enums.enum_1.הכרחי))
                {
                    _GuestRequestPool.Add(item);
                }
            }
            listBox_HostingUnit_select.ItemsSource = GuestRequestPool;           
        }

        private void Hosting3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _HostingUnitName = bl.AllHostingInJerusalem();
                listBox_HostingUnit_select.ItemsSource = HostingUnitName;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hosting4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _HostingUnitName = bl.AllHostingInCenter();
                listBox_HostingUnit_select.ItemsSource = HostingUnitName;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hosting5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _HostingUnitName = bl.AllHostingInNorth();
                listBox_HostingUnit_select.ItemsSource = HostingUnitName;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hosting6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _HostingUnitName = bl.AllHostingInSouth();
                listBox_HostingUnit_select.ItemsSource = HostingUnitName;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hosting2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _HostingUnitName = bl.MostWantedHostingUnit();
                listBox_HostingUnit_select.ItemsSource = HostingUnitName;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
