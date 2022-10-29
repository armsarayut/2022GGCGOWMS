using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Models;
using GoWMS.Server.Data;

namespace GoWMS.Server.Reports
{
    public class ggcPalletTag4x4Pdf : PdfPageEvents
    {
        readonly BaseFont mpdfFont = BaseFont.CreateFont(VarGlobals.Fontreport(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        #region GeneratePDF
        public byte[] ExportPDF(List<ggcPallet4x2> ListRpts)
        {
            BaseFont baseFont = mpdfFont;
            iTextSharp.text.Font fonheader = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);

            ////                    Set paper                        (4" , 2") Note 1" = 2.54 cm = 72
            Document doc = new Document(new iTextSharp.text.Rectangle(288, 144), 5, 5, 1, 1);
            MemoryStream ms = new MemoryStream();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                int i = 0;

                foreach (var listRpt in ListRpts)
                {
                    if (i != 0)
                        doc.NewPage();

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                    bc.TextAlignment = Element.ALIGN_CENTER;
                    bc.Code = listRpt.Palletcode.ToString();
                    bc.StartStopText = false;
                    bc.CodeType = iTextSharp.text.pdf.Barcode128.CODE128;
                    bc.Extended = true;
                    bc.Font = null;
                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);
                    cb.SetTextMatrix(45.0f, 60.0f);
                    img.ScaleToFit(175, 350);
                    img.SetAbsolutePosition(55.36f, 60.0f);
                    img.Alignment = Element.ALIGN_LEFT;
                    cb.AddImage(img);

                    PdfContentByte cb13 = writer.DirectContent;
                    cb13.BeginText();
                    cb13.SetFontAndSize(baseFont, 18.0f);
                    cb13.ShowTextAligned(Element.ALIGN_CENTER, listRpt.Palletcode.ToString(), 144f, 40f, 0);
                    cb13.EndText();

                    //PdfContentByte cb00 = writer.DirectContent;
                    //Phrase phrasePallet = new Phrase(listRpt.Palletcode.ToString());
                    //phrasePallet.Font.Size = fonheader.Size;
                    //phrasePallet.Font = fonheader;
                    //ColumnText ctPallet = new ColumnText(cb00);
                    //ctPallet.SetSimpleColumn(35f, 45f, 110f, 175f);
                    //ctPallet.AddElement(phrasePallet);
                    //ctPallet.Go();


                    i++;
                }
            }
            catch
            {
            }
            finally
            {
                doc.Close();
            }
            byte[] buff = ms.ToArray();
            return buff;

            #endregion
        }
    }
}
