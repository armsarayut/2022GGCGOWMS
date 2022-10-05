using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Models.Inb;
using GoWMS.Server.Data;

namespace GoWMS.Server.Pdf
{
    public class PdfInboundReport
    {
        readonly BaseFont mpdfFont = BaseFont.CreateFont(VarGlobals.Fontreport(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED); // Setup Font
        #region Declararion
        int _maxcolum = 8; // Setup Colum table report
        Document _document;
        PdfPTable _pdfTable = new PdfPTable(8);
        PdfPCell _pdfCell;
        Font _fontstye;
       
        MemoryStream _memoryStream = new MemoryStream();
        List<Inb_Goodreceive_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceive_Go>();
  
        #endregion
        public byte[] Report(List<Inb_Goodreceive_Go> Inb_Goodreceive_Go_s)
        {
            _Inb_Goodreceive_Go_s = Inb_Goodreceive_Go_s;

            _document = new Document(PageSize.A4, 10f, 10f, 30f, 20f);// Setup page
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfWriter.GetInstance(_document, _memoryStream);
            this.ReportHeader();
            this.ReportFooter();
            _document.Open();
            this.ReportLogo();

            float[] sizes = new float[_maxcolum];
            for (var i = 0; i < _maxcolum; i++) // Setup Colum Size
            {
                if (i == 0) sizes[i] = 50;
                else sizes[i] = 100;
            }
            _pdfTable.SetTotalWidth(sizes);
            this.ReportBody();
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();
        }

        private void ReportHeader()
        {
            //  Header
            _fontstye = new Font(mpdfFont, 12f, 1);
            var labelHeader = new Chunk("Good Receive", _fontstye);
            HeaderFooter header = new HeaderFooter(new Phrase(labelHeader), false)
            {
                Alignment = Element.ALIGN_CENTER
            };
            _document.Header = header;
        }
        private void ReportFooter()
        {
            // Footer
            _fontstye = new Font(mpdfFont, 10f, 0);
            var labelFooter = new Chunk("Page ", _fontstye);
            HeaderFooter footer = new HeaderFooter(new Phrase(labelFooter), true)
            {
                Border = Rectangle.NO_BORDER,
                Alignment = Element.ALIGN_CENTER
            };
            _document.Footer = footer;
        }
        private void ReportLogo()
        {
            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(GoWMS.Server.Data.VarGlobals.Imagelogoreport());
            png.ScaleAbsolute(40, 30);
            png.SetAbsolutePosition(10, 800);
            _document.Add(png);
        }

        private void ReportBody()
        {
            _fontstye = new Font(mpdfFont, 9f, 0); // FontFactory.GetFont("Tahoma", 9f, 1);
            #region Table Header
            _pdfCell = new PdfPCell(new Phrase("No.", _fontstye));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LightGray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Masterpallet", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Ducument", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("PackID", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Material", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Description", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Qty", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Unit", _fontstye))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LightGray
            };
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
            #endregion

            #region Table Body
            _fontstye = new Font(mpdfFont, 9f, 0); // FontFactory.GetFont("Tahoma", 9f, 1);
            int nSL = 1;
            foreach (var Inb_Goodreceive_Go_s in _Inb_Goodreceive_Go_s)
            {
                _pdfCell = new PdfPCell(new Phrase(nSL++.ToString(), _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Pallteno, _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Docno, _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Itemtag, _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Itemcode, _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Itemname, _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Quantity.ToString(), _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Inb_Goodreceive_Go_s.Unit, _fontstye))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White
                };
                _pdfTable.AddCell(_pdfCell);

                _pdfTable.CompleteRow();
            }
            #endregion
        }
    }
}
