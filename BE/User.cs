using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RefId { get; set; }
        public UserTypes UserType { get; set; } = UserTypes.Host;

        public static implicit operator User(XElement v)
        {
            throw new NotImplementedException();
        }
    }

    public enum UserTypes
    {
        Host,
        Guest,
        administrator
    }
}
