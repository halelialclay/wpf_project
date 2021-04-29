using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BE
{
    public class HostingUnit //מחלקה שמייצגת יחידת אירוח
    {
        public int HostingUnitKey { get; set; } = 0;
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }
        public Enums.Yes_No WindowToTheSea { get; set; } = Enums.Yes_No.No;//חלון לים
        public Enums.Yes_No ThereIsPool { get; set; } = Enums.Yes_No.No;//האם יש בריכה
        public Enums.Yes_No DisabledAccessible { get; set; } = Enums.Yes_No.No;//נגיש לנכים
        public Enums.Yes_No childrensAttractions { get; set; } = Enums.Yes_No.No;// האם יש אטרקציות לילדים
        public Enums.Yes_No jacuzzi { get; set; } = Enums.Yes_No.No;//האם יש ג'קוזי
        public Enums.Area area { get; set; } = Enums.Area.South;//איזור
        public string subArea { get; set; }
        public Enums.ResortType resortType { get; set; }
        public int numberOfPlaces { get; set; } = 2;
        public  int Commission { get; set; } = 0;

        [XmlIgnore]
        public bool[,] Diary = new bool[12, 31];

        [XmlArray("Diary")]
        public bool[] DiaryDto
        {
            get { return Diary.Flatten(); }
            set
            {
                Diary = value.Expand(12);
            }
        }

        public override string ToString()
        {
            return this.ToStringProperty();       
        }
    }
}
