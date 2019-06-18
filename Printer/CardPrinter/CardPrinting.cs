using Printer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Text;

namespace Printer.CardPrinter
{
    public class CardPrinting : IPrinting
    {
        private CardModel _model;
        private PrivateFontCollection _privateFontCollection;
        private int _maxLenghtOfString = 45;

        public void Print(dynamic model, string printerName, PrivateFontCollection privateFontCollection = null)
        {
            _model = new CardModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ID = model.ID,
                Street = model.Street,
                City = model.City,
                MemberImage = model.MemberImage,
                LogoImage = model.LogoImage,
                WWWAddress = model.WWWAddress
            };

            _privateFontCollection = privateFontCollection;

            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = printerName;
            pd.OriginAtMargins = true;
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
            pd.Print();
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            var yrsa = Array.Find(_privateFontCollection.Families, item => item.Name == "Yrsa");

            Font mediumFont = new Font(yrsa, 9);
            Font regularFont = new Font(yrsa, 10);
            Font boldFont = new Font(yrsa, 10);

            PointF fullnamePoint = new PointF(35, 21);
            PointF namePoint = new PointF(35, 21);
            PointF surnamePoint = new PointF(35, 25);
            PointF idPoint = new PointF(35, 25);
            PointF myClubPoint = new PointF(35, 33);
            PointF streetPoint = new PointF(35, 37);
            PointF cityPoint = new PointF(35, 41);

            Graphics graphics = e.Graphics;
            graphics.PageUnit = GraphicsUnit.Millimeter;

            SolidBrush orangeColor = new SolidBrush(Color.FromArgb(237, 106, 18));

            DrawWebAddress(e, yrsa, graphics);

            if (_model.LogoImage != null)
            {
                graphics.DrawImage(_model.LogoImage, new Rectangle(53, 5, 27, 7), new Rectangle(0, 0, _model.LogoImage.Width, _model.LogoImage.Height), GraphicsUnit.Pixel);
            }
            if (_model.MemberImage != null)
            {
                graphics.DrawImage(_model.MemberImage, new Rectangle(5, 17, 24, 32), new Rectangle(0, 0, _model.MemberImage.Width, _model.MemberImage.Height), GraphicsUnit.Pixel);
            }


            graphics.DrawString($"Name:", mediumFont, orangeColor, new PointF(35, 17));

            DrawMemberSection(e, boldFont, fullnamePoint, namePoint, surnamePoint, ref idPoint, ref myClubPoint, ref streetPoint, ref cityPoint, graphics);

            graphics.DrawString($"ID: {_model.ID.ToString()}", regularFont, Brushes.Black, idPoint);

            graphics.DrawString("My address", mediumFont, orangeColor, myClubPoint);
            graphics.DrawString(_model.Street, regularFont, Brushes.Black, streetPoint);
            graphics.DrawString(_model.City, regularFont, Brushes.Black, cityPoint);
        }

        private void DrawMemberSection(PrintPageEventArgs e, Font boldFont, PointF fullnamePoint, PointF namePoint,
            PointF surnamePoint, ref PointF idPoint, ref PointF myClubPoint, ref PointF streetPoint, ref PointF cityPoint, Graphics graphics)
        {
            SizeF fullNameSize = new SizeF();
            fullNameSize = e.Graphics.MeasureString(_model.FullName, boldFont);

            if (fullNameSize.Width > _maxLenghtOfString)
            {
                SplitSurname(e, boldFont, out idPoint, ref myClubPoint, ref streetPoint, ref cityPoint, graphics, out string surname);

                graphics.DrawString(_model.FirstName, boldFont, Brushes.Black, namePoint);
                graphics.DrawString(surname, boldFont, Brushes.Black, surnamePoint);
            }
            else
            {
                graphics.DrawString(_model.FullName, boldFont, Brushes.Black, fullnamePoint);
            }
        }

        private void SplitSurname(PrintPageEventArgs e, Font boldFont,
            out PointF idPoint, ref PointF myClubPoint, ref PointF streetPoint, ref PointF cityPoint, Graphics graphics, out string surname)
        {
            SizeF surnameSize = new SizeF();
            surnameSize = e.Graphics.MeasureString(_model.LastName, boldFont);
            surname = string.Empty;


            if (surnameSize.Width > _maxLenghtOfString)
            {
                string surnameParts = string.Empty;
                string secondRow = string.Empty;

                SizeF wordSize = new SizeF();
                string[] lastName = _model.LastName.Split(' ');
                for (int i = 0; i < lastName.Length; i++)
                {
                    surnameParts += lastName[i];
                    wordSize = e.Graphics.MeasureString(surnameParts, boldFont);

                    if (wordSize.Width > _maxLenghtOfString)
                    {
                        secondRow += $" {lastName[i]}";
                    }
                    else
                    {
                        surname += $" {lastName[i]}";
                    }
                }
                if (!string.IsNullOrEmpty(secondRow))
                {
                    secondRow = secondRow.TrimStart(' ');
                    graphics.DrawString(secondRow, boldFont, Brushes.Black, new PointF(35, 29));
                }
                surname = surname.TrimStart(' ');

                idPoint = new PointF(35, 33);
                myClubPoint = new PointF(35, 37);
                streetPoint = new PointF(35, 41);
                cityPoint = new PointF(35, 45);
            }
            else
            {
                surname = _model.LastName;
                idPoint = new PointF(35, 29);
            }
        }

        private void DrawWebAddress(PrintPageEventArgs e, FontFamily mediumNiveau, Graphics graphics)
        {
            SizeF characterSize = new SizeF();
            if (!string.IsNullOrEmpty(_model.WWWAddress))
            {
                char[] wwwCharacters = _model.WWWAddress.ToCharArray();
                float pointX = 5;
                for (int i = 0; i < wwwCharacters.Length; i++)
                {
                    characterSize = e.Graphics.MeasureString(wwwCharacters[i].ToString(), new Font(mediumNiveau, 7));
                    graphics.DrawString(wwwCharacters[i].ToString(), new Font(mediumNiveau, 7), Brushes.Black, new PointF(pointX, 5));
                    pointX += characterSize.Width - 0.4f;
                }
            }
        }
    }
}
