using System.Drawing.Text;

namespace Printer
{
    public interface IPrinting
    {
        void Print(dynamic model, string printerName, PrivateFontCollection privateFontCollection = null);
    }
}
