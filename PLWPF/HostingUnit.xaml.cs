using BE;
using BL;
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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for hostingUnit.xaml
    /// </summary>
    public partial class HostingUnit : Window
    {
        public OrderManagement orderManagement;// = new OrderManagement();
        BL.BL_imp bl = BL.FactoryBl.GetIBL();
        BE.HostingUnit unit = new BE.HostingUnit();
//קונסטרקטור
        public HostingUnit()
        {
            InitializeComponent();

            HostingUnit_.Items.Clear();// = null;
            try
            {
                HostingUnit_.ItemsSource = bl.GetHostingUnits(BL.BL_imp.loggedInHost.HostKey);
            }
            catch(ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            typeUnit.ItemsSource = Enum.GetValues(typeof(BE.Enums.ResortType)).Cast<BE.Enums.ResortType>();
            MainArea.ItemsSource = Enum.GetValues(typeof(Enums.Area)).Cast<Enums.Area>();
         
            DataContext = this;
        }

     
        //איתחול משתנים
         
        public List<BE.HostingUnit> MyHostingUnits
        {
            get
            {
                    return bl.GetHostingUnits(BL.BL_imp.loggedInHost.HostKey);
           
            }
        }
        
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.HostingUnit unit = (BE.HostingUnit)((DataGrid)sender).SelectedItem;
        }


        private void UnitDetails(object sender, RoutedEventArgs e)
        {
          BE.HostingUnit unit = (BE.HostingUnit)((Button)sender).DataContext;
             MessageBox.Show(unit.ToString(), "פרטים", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }
        //הוספת יחידה
        private void addUnit_Click(object sender, RoutedEventArgs e)
        {
            newHostingUnit.DataContext = new BE.HostingUnit();
            HostingUnit_.Visibility = Visibility.Hidden;
            addUnit.Visibility = Visibility.Hidden;
            newHostingUnit.Visibility = Visibility.Visible;
        }
        private void MainArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
          
            SelectSecArea();
        }

        private void SelectSecArea()
        {
            SecArea.ItemsSource = BE.Enums.areas[(Enums.Area)MainArea.SelectedItem];
        }
        //כפתור שמירה
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            BE.HostingUnit unit = (BE.HostingUnit)newHostingUnit.DataContext;
            if (unit.HostingUnitKey == 0)
            {
                HostingUnit_.ItemsSource = null;
                HostingUnit_.ItemsSource = MyHostingUnits;
                try
                {
                    BL.BL_imp.loggedInUser.RefId= bl.CreateHostingUnit(unit, BL.BL_imp.loggedInHost);
                    bl.updateUser(BL.BL_imp.loggedInUser.Id, BL.BL_imp.loggedInUser.RefId);

                    HostingUnit_.ItemsSource = null;
                    HostingUnit_.ItemsSource = MyHostingUnits;
                }
                catch (ZimmerException a)
                {

                    MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                 
                    MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                try
                {
                    bl.UpdateHostingUnit(unit);
                    HostingUnit_.ItemsSource = null;
                    HostingUnit_.ItemsSource = MyHostingUnits;
                }
                catch (BE.ZimmerException a)
                {
                    MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                HostingUnit_.Visibility = Visibility.Visible;
            addUnit.Visibility = Visibility.Visible;
            newHostingUnit.Visibility = Visibility.Hidden;
        }

        //private Enums.Yes_No choiceRadio(UIElementCollection children)
        //{
        //    for (int i = 0; i < children.Count; i++)
        //    {
        //        if (children[i] is RadioButton && ((RadioButton)children[i]).IsChecked.Value)
        //        {
        //            if (i == 0)
        //                return Enums.Yes_No.No;
        //            else if (i == 1)
        //                return Enums.Yes_No.Yes;                  
        //        }
        //    }
        //    return Enums.Yes_No.No;
        //}

      

        public List<BE.HostingUnit> MyHost
        {
            get
            {
                return bl.ReadAllHostingUnit();
            }
        }
        //עדכון יחידה
        private void updateUnit(object sender, RoutedEventArgs e)
        {
            newHostingUnit.DataContext = (BE.HostingUnit)((Button)sender).DataContext;
            newHostingUnit.Visibility = Visibility.Visible;
            HostingUnit_.Visibility = Visibility.Hidden;
            addUnit.Visibility = Visibility.Hidden;
            HostingUnit_.ItemsSource = null;
            HostingUnit_.ItemsSource = MyHostingUnits;
        }
        //מחיקת יחידה
        private void deleteUnit(object sender, RoutedEventArgs e)
        {
            try
            {
                unit = (BE.HostingUnit)((Button)sender).DataContext;
                bl.DeleteHostingUnit(unit.HostingUnitKey);
                HostingUnit_.ItemsSource = null;
                HostingUnit_.ItemsSource = MyHostingUnits;
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static List<BE.Order> orders;
        //פתיחת חלון הזמנות
        private void Order_Management(object sender, RoutedEventArgs e)
        {
           
            unit = (BE.HostingUnit)((Button)sender).DataContext;
            
            BL_imp.loggedInUser.RefId = unit.HostingUnitKey;
            orderManagement = new OrderManagement();
            
            orderManagement.ShowDialog();
            
            orderManagement.Close();
        }

        private void cancel_clik(object sender, RoutedEventArgs e)
        {
            newHostingUnit.Visibility = Visibility.Hidden;
            HostingUnit_.Visibility = Visibility.Visible;
            addUnit.Visibility = Visibility.Visible;
        }

    
    }
}
