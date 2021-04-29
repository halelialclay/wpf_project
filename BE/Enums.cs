using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public class Enums //מחלקה שכוללת את כל הטיפוסים הסודרים
    {
        public enum ResortType { Zimmer , guestRoom, HotelRoom, encampment}
        public enum Area {North, South, Center, Jerusalem}
        //public enum SubArea {TelAviv, Tiberias, Eilat}
        public enum OrderStatus { סגירת_עיסקה,טרם_טופל, נשלח_מייל, נסגר_מחוסר_הענות_הלקוח,פג_תוקפה}
        public enum CustomerRequirementStatus { open,closed,expired}
        public enum enum_1 {הכרחי,אפשרי,לא_מעוניין}
        public enum Yes_No { No, Yes}

        public static Dictionary<Area, List<string>> areas = new Dictionary<Area, List<string>>()
        {
            {
                Area.North, new List<string>()
                {
                    "צפת", "חיפה","טבריה"
                }
            },
            {
                Area.South, new List<string>()
                {
                    "אילת","מצפה רמון","באר שבע"
                }
            },
            {
                Area.Center, new List<string>()
                {
                    "תל-אביב","נתניה","הרצליה"
                }
            },
            { 
                Area.Jerusalem, new List<string>()
                {
                    "ירושלים", "מודיעין","כפר אדומים"
                }
            }
        };
    }
}