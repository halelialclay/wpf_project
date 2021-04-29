using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderManagement.xaml
    /// </summary>
    public partial class OrderManagement : Window
    {
       
        BL.BL_imp bl = BL.FactoryBl.GetIBL();
        //private List<Order> or;

        public OrderManagement()
        {
            InitializeComponent();
            DataContext = this;
            bl.ReadAllOrder().Where(v => v.HostingUnitKey == BL_imp.loggedInUser.RefId).ToList();

        }
        public List<BE.Order> MyOrder
        {
            get
            {
                return bl.ReadAllOrder().Where(v => v.HostingUnitKey == BL_imp.loggedInUser.RefId && v.orderStatus == Enums.OrderStatus.טרם_טופל).ToList();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    

        private void OrderDetails(object sender, RoutedEventArgs e)
        {
            BE.Order order= (BE.Order)((Button)sender).DataContext;
            MessageBox.Show(order.ToString(), "פרטים", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Order order = (BE.Order)((DataGrid)sender).SelectedItem;
        }
        //שליחת מייל
        private void mail_click(object sender, RoutedEventArgs e)
        {
            MailMessage mail = new MailMessage();
            BE.Order order = (BE.Order)((Button)sender).DataContext;
            string HostMail = "hllydn7@gmail.com";// BL.BL_imp.loggedInUser.Email;
            BE.GuestRequest guest = bl.ReadAllGuestRequest().Where(x => x.GuestRequestKey == order.GuestRequestKey).FirstOrDefault();
            string GuestMail = guest.MailAddress;
            string PasswordMail = "hhhaaalll";//BL.BL_imp.loggedInUser.Password;

            mail.To.Add(GuestMail);
            mail.From = new MailAddress(HostMail);
            mail.Subject = "הצעת ארוח ממערכת גן עדן";
            BE.HostingUnit hostingUnit = bl.ReadHostingUnit(order.HostingUnitKey);
            mail.Body = GetMailBody(guest, hostingUnit);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(HostMail, PasswordMail);
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            
             }
            ChangeStatus(order);
        }
        //גוף ההודעה
        private string GetMailBody(GuestRequest guest, BE.HostingUnit unit)
        {
            return string.Format(@"
                    <html>
                        <body>
                            שלום {0} ,<br/>
                            נמצאה התאמה מיחידה  {1}
                        </body>
                    </html>
            ", guest.PrivateName + " " + guest.FamilyName,
            unit.HostingUnitName);
        }
        //שינוי סטטוס
        private void ChangeStatus(Order order)
        {
            order.orderStatus = (BE.Enums.OrderStatus) BE.Enums.OrderStatus.נשלח_מייל;
            order.OrderDate = DateTime.Now;
            bl.UpdateOrder(order);

            this.Order_.ItemsSource = MyOrder;
        }

    }

}
