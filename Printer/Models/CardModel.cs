using System.Drawing;

namespace Printer.Models
{
    public class CardModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string WWWAddress { get; set; }
        public Image MemberImage { get; set; }
        public Image LogoImage { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }
    }
}
