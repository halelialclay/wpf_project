using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Enums;

namespace BE
{
    public class Host //מחלקה שמייצגת מארח
    {
        public int HostKey { get; set; }

        //[Required]
        //[StringLength(30)]
        public string PrivateName { get; set; }

        //[Required]
        //[StringLength(30)]
        public string FamilyName { get; set; }

        public Int64 PhoneNumber { get; set; }

        //[Required]
        //[StringLength(250)]
        public string MailAddress { get; set; }

        public String BankBranchDetails { get; set; }

        //[Required]
        public int BankAccountNumber { get; set; }

        public  Yes_No CollectionClearance=Yes_No.No;

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }  
}
