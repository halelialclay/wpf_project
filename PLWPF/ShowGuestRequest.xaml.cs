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
using System.Net.Mail;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ShowGuestRequest.xaml
    /// </summary>
    public partial class ShowGuestRequest : Window
    {
        public ShowGuestRequest()
        {
            InitializeComponent();
        }
        BL.BL_imp bl = BL.FactoryBl.GetIBL();
        public List<BE.Order> Allorder
        {
            get
            {
                return null;//bl.ReadAllOrder().Where(x=>x.HostingUnitKey==order);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Order unit = (BE.Order)((DataGrid)sender).SelectedItem;
        }

    

        private void Mail_click(object sender, RoutedEventArgs e)
        {
            MailMessage mail = new MailMessage();
            BE.GuestRequest guest = (BE.GuestRequest)((Button)sender).DataContext;
            string HostMail = BL.BL_imp.loggedInUser.Email;
            string GuestMail = guest.MailAddress;
            string PasswordMail = BL.BL_imp.loggedInUser.Password;

            string Subject ="   ";
            mail.To.Add(GuestMail);
            mail.From = new MailAddress(HostMail);
            mail.Subject = "    ";
            mail.Body = "mailBody";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(HostMail, PasswordMail);
             smtp.EnableSsl = true;

         
        }


   

        private void out_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void updateUnit(object sender, RoutedEventArgs e)
        {

        }
    }
}
