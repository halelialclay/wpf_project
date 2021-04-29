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
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// 
    public partial class LoginWindow : Window
    {
        private bool _isNewUser = false;
        private CookieContainer c1;

        public bool isNewUser
        {
            get { return _isNewUser; }
        }
        //private User _loggeninNewUser;
        //public User LoggeninNewUser
        //{
        //    get { return _loggeninNewUser; }
        //}

        //public User loggeninUser { get; }

        public LoginWindow()
        {
            InitializeComponent();
        }
        //כניסת משתנה קיים
        private void Button_Old_User(object sender, RoutedEventArgs e)
        {
            BL.BL_imp bl = new BL.BL_imp();
            try
            {
                BL_imp.loggedInUser = bl.Login(LUserName.Text, LPassword.Password);
            }
            catch (BE.ZimmerException a)
            {
                MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (BL_imp.loggedInUser == null)
                loginError.Visibility = Visibility.Visible;
            else
            {
                if (BL_imp.loggedInUser.UserType == BE. UserTypes.Host)
                    try
                    {
                        BL_imp.loggedInHost = bl.ReadHostingUnit(BL_imp.loggedInUser.RefId).Owner;
                    }
                    catch (BE.ZimmerException a)
                    {
                        MessageBox.Show(a.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                _isNewUser = false;
                DialogResult = true;
                Close();
            }
        }
 //כניסה למשתנה חדש
        private void Button_New_User(object sender, RoutedEventArgs e)
        {
            BL.BL_imp bl = new BL.BL_imp();
         User users=   bl.ReadAllUser().Where(x=>x.Username== rusernametb.Text).FirstOrDefault();
           
                User ruser = new User()
                {
                    Username = rusernametb.Text,
                    Password = rpasswordtb.Password,
                    Email = rusernametb_Copy1.Text,
                    UserType = (LogInTheSiteHost.IsChecked.Value) ? UserTypes.Host : UserTypes.Guest,
                };



                if (!emailGood(ruser.Email))
                    EmailError.Visibility = Visibility.Visible;
                else
                {
                    EmailError.Visibility = Visibility.Hidden;

                    bool res = bl.register(ruser);
                    BL_imp.loggedInUser = ruser;
                    DialogResult = (BL_imp.loggedInUser != null);
                    Close();
                }
                _isNewUser = true;
           
        }
        //יציאה
        private void Button_out(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //בדיקת תקינות מייל
        public static bool emailGood(string email)
        {
            if ( (email.IndexOf('@') > 0) && (email.IndexOf('.') > 0))
            {
                return true;
            }
            return false;
        }
        //הודעת שגיאה ברמת ה  ui
        private void rusernametb_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!emailGood(rusernametb_Copy1.Text))
            {
                EmailError.Visibility = Visibility.Visible;
            }
            else
            {
                EmailError.Visibility = Visibility.Hidden;
            }
        }
   
        private void LogInTheSiteHost_Checked(object sender, RoutedEventArgs e)
        {

        }
        
        private void ChangeText(object sender, MouseButtonEventArgs e)
        {
            LUserName.Clear();
        }

        private void ChangeText1(object sender, MouseButtonEventArgs e)
        {
            rusernametb.Clear();
        }

    }
}
