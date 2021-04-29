using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    public class GuestRequest //מחלקה שמייצגת דרישת אירוח של לקוח
    {
        [Range(10000001, 99999999)]
        public int GuestRequestKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public Enums.CustomerRequirementStatus status { get; set; } = Enums.CustomerRequirementStatus.open;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public  Enums.Area area { get; set; }
        public  string subArea { get; set; }
        public  Enums.ResortType type { get; set; }
        public int Adults { get; set; } = 2;
        public int Children { get; set; } = 0;
        public int numberOfGuest { get; set; } = 2;
        public Enums.enum_1 WindowToSea { get; set; } = Enums.enum_1.לא_מעוניין;
        public Enums.enum_1 pool { get; set; } = Enums.enum_1.אפשרי;
        public  Enums.enum_1 garden { get; set; } = Enums.enum_1.אפשרי;
        public  Enums.enum_1 childrensAttractions { get; set; } = Enums.enum_1.אפשרי;
        public  Enums.enum_1 jacuzzi { get; set; } = Enums.enum_1.אפשרי;
        public Enums.enum_1 DisabledAccessible { get; set; } = Enums.enum_1.אפשרי;

        public override string ToString()
        {
            return this.ToStringProperty();
        }

        public static implicit operator GuestRequest(XElement v)
        {
            throw new NotImplementedException();
        }
    }
}
