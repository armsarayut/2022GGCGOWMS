using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using GoWMS.Server.Data;

namespace GoWMS.Server.Reports
{
    public class PdfPageEvents : PdfPageEventHelper
    {
        Image Logo;

        readonly string pathImage = GoWMS.Server.Data.VarGlobals.Imagelogoreport();

        readonly BaseFont mpdfFont = BaseFont.CreateFont(VarGlobals.Fontreport(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED); // Setup Font

        #region Attributes
        private String _fontFilePathForHeaderFooter = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "SIMHEI.TTF");
        /// <summary>
        /// Font used for header/footer
        /// </summary>
        /// <returns></returns>
        public String FontFilePathForHeaderFooter
        {
            get
            {
                return _fontFilePathForHeaderFooter;
            }

            set
            {
                _fontFilePathForHeaderFooter = value;
            }
        }

        private String _fontFilePathForBody = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "SIMSUN.TTC,1");
        /// <summary>
        /// Font used for body content
        /// </summary>
        public String FontFilePathForBody
        {
            get { return _fontFilePathForBody; }
            set { _fontFilePathForBody = value; }
        }


        private PdfPTable _header;
        /// <summary>
        /// Top of page
        /// </summary>
        /// <returns></returns>
        public PdfPTable Header
        {
            get { return _header; }
            private set { _header = value; }
        }

        private PdfPTable _footer;
        /// <summary>
        /// Footer
        /// </summary>
        /// <returns></returns>
        public PdfPTable Footer
        {
            get { return _footer; }
            private set { _footer = value; }
        }


        private BaseFont _baseFontForHeaderFooter;
        /// <summary>
        /// Font used at the end of the first page
        /// </summary>
        /// <returns></returns>
        public BaseFont BaseFontForHeaderFooter
        {
            get { return _baseFontForHeaderFooter; }
            set { _baseFontForHeaderFooter = value; }
        }

        private BaseFont _baseFontForBody;
        /// <summary>
        /// Font used in the body
        /// </summary>
        /// <returns></returns>
        public BaseFont BaseFontForBody
        {
            get { return _baseFontForBody; }
            set { _baseFontForBody = value; }
        }

        private Document _document;
        /// <summary>
        /// PDF Document
        /// </summary>
        /// <returns></returns>
        public Document Document
        {
            get { return _document; }
            private set { _document = value; }
        }

        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                Logo = iTextSharp.text.Image.GetInstance(pathImage);
                Logo.ScaleAbsolute(65f, 35f);

                Logo.SetAbsolutePosition(document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 10);

                document.Add(Logo);

                BaseFontForHeaderFooter = mpdfFont;
                BaseFontForBody = mpdfFont;
                Document = document;
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        #region GenerateHeader
        /// <summary>
        /// Generate header
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual PdfPTable GenerateHeader(iTextSharp.text.pdf.PdfWriter writer)
        {
            return null;
        }
        #endregion

        #region GenerateFooter
        /// <summary>
        /// Generate footer
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual PdfPTable GenerateFooter(iTextSharp.text.pdf.PdfWriter writer)
        {
            return null;
        }
        #endregion

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            writer.DirectContent.AddImage(Logo);

            //Output page top
            Header = GenerateHeader(writer);
            Header.TotalWidth = document.PageSize.Width - 20f;
            Header.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 20, writer.DirectContent);

            //Output footer
            Footer = GenerateFooter(writer);
            Footer.TotalWidth = document.PageSize.Width - 20f;
            Footer.WriteSelectedRows(0, -1, 10, document.PageSize.GetBottom(20), writer.DirectContent);
        }
    }
}
