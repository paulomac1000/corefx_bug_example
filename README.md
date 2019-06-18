# corefx_bug_example
System.Drawing - printing bug presentation

Error on Linux:
root@debian:/home/pablo/Projects/corefx_bug/Printer# dotnet run Printer.csproj -PDF

Unhandled Exception: System.NullReferenceException: Object reference not set to an instance of an object.
   at System.Drawing.Printing.PrintingServices.LoadPrinterSettings(String printer, PrinterSettings settings)
   at Printer.CardPrinter.CardPrinting.Print(Object model, String printerName, PrivateFontCollection privateFontCollection) in /home/pablo/Projects/corefx_bug/Printer/CardPrinter/CardPrinting.cs:line 34
   at Printer.Program.Main(String[] args) in /home/pablo/Projects/corefx_bug/Printer/Program.cs:line 32

On Windows printing works normally.


