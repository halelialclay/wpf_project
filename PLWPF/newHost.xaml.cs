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
    /// Interaction logic for newHost.xaml
    /// </summary>
    public partial class newHost : Window
    {
        public newHost()
        {
            InitializeComponent();
            DataContext = this;
            save.Click += ttt;
        }

        private void ttt(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private BL.BL_imp bl = BL.FactoryBl.GetIBL();
        //שמירת פרטים
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            BL_imp.loggedInHost = new Host();
            BL_imp.loggedInHost.PrivateName = lineFirstName.Text;
            BL_imp.loggedInHost.FamilyName = lineFamilyName.Text;
            BL_imp.loggedInHost.MailAddress = BL_imp.loggedInUser.Email;
            BL_imp.loggedInHost.PhoneNumber = Convert.ToInt64(linePhone.Text);
            BL_imp.loggedInHost.BankBranchDetails =(string) bank.SelectedItem;
            BL_imp.loggedInHost.BankAccountNumber = Convert.ToInt32(lineAccountNumber.Text);
            BL_imp.loggedInHost.CollectionClearance =(Enums.Yes_No) enumsYesNo.SelectedItem;
            Close();
        }

        private void Bank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public List<string> BankList
        {
            get
            {
                return bl.GetListBankAccount().Select(y=>y.BankName+" "+y.BranchNumber).ToList();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            enumsYesNo.ItemsSource = Enum.GetValues(typeof(Enums.Yes_No)).Cast<Enums.Yes_No>();
        }

        private void Bank_Loaded(object sender, RoutedEventArgs e)
        {
            //bank.SelectedItem = List.NameProperty(typeof(list<BankAccount>));
        }

        private void EnumsYesNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }    
    }
}
