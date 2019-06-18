using Printer.CardPrinter;
using Printer.Models;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Printer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CardPrinting _cardPrinting = new CardPrinting();

            var model = new CardModel()
            {
                FirstName = "Theresa",
                LastName = "Fengler",
                ID = 579029,
                Street = "Brenneckestrabe 46",
                City = "39118 Magdeburg",
                WWWAddress = "WWW.ADRESS.COM"
            };
            string printerName = args != null && args.Any() ? args[0] : "Foxit Reader PDF Printer";

            model.MemberImage = Image.FromFile($"{RootDir}/Content/person.jpg");
            model.LogoImage = Image.FromFile($"{RootDir}/Content/logo.png");

            _cardPrinting.Print(model, printerName, GetFonts());
        }

        private static string RootDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
             .Replace(@"file:\\", string.Empty).Replace(@"file:\", string.Empty)
             .Replace(@"file:", string.Empty);

        private static PrivateFontCollection GetFonts()
        {
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            privateFontCollection.AddFontFile($"{RootDir}/Content/Yrsa-Regular.ttf");
            return privateFontCollection;
        }
    }
}