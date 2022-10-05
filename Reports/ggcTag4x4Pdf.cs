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
    public class ggcTag4x4Pdf : PdfPageEvents
    {

        readonly BaseFont mpdfFont = BaseFont.CreateFont(VarGlobals.Fontreport(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //// Header -------------------------------------
        string hPallet = "Pallet No.";
        string hTag = "Tag No.";
        string hBatch = "Batch No :";
        string hProduct = "Product :";
        string hQty = "Qty Per Pallet";
        string hPackaging = "Packaging :";
        string hPackdate = "Packing Date :";
        string hReservation = "Reservation :";
        string hCreate = "Create by :";
        //// -------------------------------------
        ///
        #region GeneratePDF

        public byte[] ExportPDF(List<ggcTag4x4> ListRpts)
        {


            BaseFont baseFont = mpdfFont;

            iTextSharp.text.Font fonheader = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fondata = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);
            ////                    Set paper                        (4" , 4") Note 1" = 2.54 cm = 72
            Document doc = new Document(new iTextSharp.text.Rectangle(288f, 288f), 5, 5, 1, 1);

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

                    #region Intial report form
                    //// Intial Line ------------------

                    PdfContentByte cbl = writer.DirectContent;
                    cbl.RoundRectangle(5f, 8f, 280f, 276f, 2f);
                    cbl.Stroke();

                    cbl.MoveTo(110, 88);
                    cbl.LineTo(110, 48);
                    cbl.Stroke();

                    cbl.MoveTo(110, (float)(doc.PageSize.Height - 3.5));
                    cbl.LineTo(110, 208);
                    cbl.Stroke();

                    cbl.MoveTo(5, 208);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 208);
                    cbl.Stroke();

                    cbl.MoveTo(5, 188);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 188);
                    cbl.Stroke();

                    cbl.MoveTo(5, 168);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 168);
                    cbl.Stroke();

                    cbl.MoveTo(5, 88);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 88);
                    cbl.Stroke();

                    cbl.MoveTo(110, 68);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 68);
                    cbl.Stroke();

                    cbl.MoveTo(5, 48);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 48);
                    cbl.Stroke();

                    cbl.MoveTo(5, 28);
                    cbl.LineTo((float)(doc.PageSize.Width - 3.5), 28);
                    cbl.Stroke();
                    ///----------------------------------------------------------------
                    #endregion

                    #region Add Header
                    //// Add Header -------------------

                    PdfContentByte cb00 = writer.DirectContent;
                    Phrase phrasePallet = new Phrase(hPallet);
                    phrasePallet.Font.Size = fonheader.Size;
                    phrasePallet.Font = fonheader;
                    ColumnText ctPallet = new ColumnText(cb00);
                    ctPallet.SetSimpleColumn(35f, 262f, 110f, 284f);
                    ctPallet.AddElement(phrasePallet);
                    ctPallet.Go();

                    PdfContentByte cb01 = writer.DirectContent;
                    cb01.BeginText();
                    cb01.SetFontAndSize(baseFont, 14.0f);
                    cb01.ShowTextAligned(Element.ALIGN_CENTER, hTag, 195f, 266f, 0);
                    cb01.EndText();

                    PdfContentByte cb02 = writer.DirectContent;
                    cb02.SetFontAndSize(baseFont, 12.0f);
                    cb02.BeginText();
                    cb02.SetTextMatrix(10f, 193f);
                    cb02.ShowText(hBatch);
                    cb02.EndText();

                    PdfContentByte cb03 = writer.DirectContent;
                    cb03.SetFontAndSize(baseFont, 12.0f);
                    cb03.BeginText();
                    cb03.SetTextMatrix(10f, 175f);
                    cb03.ShowText(hProduct);
                    cb03.EndText();

                    PdfContentByte cb04 = writer.DirectContent;
                    cb04.SetFontAndSize(baseFont, 10.0f);
                    cb04.BeginText();
                    cb04.SetTextMatrix(25f, 75f);
                    cb04.ShowText(hQty);
                    cb04.EndText();

                    PdfContentByte cb05 = writer.DirectContent;
                    cb05.SetFontAndSize(baseFont, 10.0f);
                    cb05.BeginText();
                    cb05.SetTextMatrix(115f, 75f);
                    cb05.ShowText(hPackaging);
                    cb05.EndText();

                    PdfContentByte cb06 = writer.DirectContent;
                    cb06.SetFontAndSize(baseFont, 10.0f);
                    cb06.BeginText();
                    cb06.SetTextMatrix(115f, 55f);
                    cb06.ShowText(hPackdate);
                    cb06.EndText();

                    PdfContentByte cb07 = writer.DirectContent;
                    cb07.SetFontAndSize(baseFont, 10.0f);
                    cb07.BeginText();
                    cb07.SetTextMatrix(10f, 35f);
                    cb07.ShowText(hReservation);
                    cb07.EndText();

                    PdfContentByte cb08 = writer.DirectContent;
                    cb08.SetFontAndSize(baseFont, 10.0f);
                    cb08.BeginText();
                    cb08.SetTextMatrix(10f, 15f);
                    cb08.ShowText(hCreate);
                    cb08.EndText();

                    ///----------------------------------------------------------------
                    #endregion

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   #region Add data
                    //// Add Data ---------------------
                    ///
                    PdfContentByte cb10 = writer.DirectContent;
                    cb10.BeginText();
                    cb10.SetFontAndSize(baseFont, 24.0f);
                    cb10.ShowTextAligned(Element.ALIGN_CENTER, listRpt.PalletNo.ToString(), 55f, 230f, 0);
                    cb10.EndText();


                    iTextSharp.text.pdf.PdfContentByte cb11 = writer.DirectContent;
                    iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                    bc.TextAlignment = Element.ALIGN_CENTER;
                    bc.Code = listRpt.TagNo.ToString();
                    bc.StartStopText = false;
                    bc.CodeType = iTextSharp.text.pdf.Barcode128.CODE128;
                    bc.Extended = true;
                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb11, iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);
                    cb11.SetTextMatrix(150.0f, 210.0f);
                    img.ScaleToFit(130, 100);
                    img.SetAbsolutePosition(133.0f, 210.0f);
                    img.Alignment = Element.ALIGN_CENTER;
                    cb11.AddImage(img);

                    PdfContentByte cb12 = writer.DirectContent;
                    cb12.BeginText();
                    cb12.SetFontAndSize(baseFont, 12.0f);
                    cb12.ShowTextAligned(Element.ALIGN_CENTER, listRpt.BatchNo.ToString() , 105f, 193f, 0);
                    cb12.EndText();

                    PdfContentByte cb13 = writer.DirectContent;
                    cb13.BeginText();
                    cb13.SetFontAndSize(baseFont, 12.0f);
                    cb13.ShowTextAligned(Element.ALIGN_CENTER, listRpt.ProductNo.ToString(), 105f, 175f, 0);
                    cb13.EndText();

                    PdfContentByte cb14 = writer.DirectContent;
                    Phrase phrase = new Phrase(listRpt.ProductName.ToString());
                    phrase.Font.Size = fondata.Size;
                    phrase.Font = fondata;
                    ColumnText ct14 = new ColumnText(cb14);
                    ct14.SetSimpleColumn(10f, 90f, 280f, 168f);
                    ct14.Alignment = Element.ALIGN_MIDDLE;
                    ct14.AddElement(phrase);
                    ct14.Go();

                    PdfContentByte cb15 = writer.DirectContent;
                    cb15.BeginText();
                    cb15.SetFontAndSize(baseFont, 10.0f);
                    cb15.ShowTextAligned(Element.ALIGN_CENTER, listRpt.QtyPerPallet.ToString() , 55f, 55f, 0);
                    cb15.EndText();

                    PdfContentByte cb16 = writer.DirectContent;
                    cb16.BeginText();
                    cb16.SetFontAndSize(baseFont, 10.0f);
                    cb16.ShowTextAligned(Element.ALIGN_CENTER, listRpt.Packaging.ToString(), 175f, 75f, 0);
                    cb16.EndText();

                    PdfContentByte cb17 = writer.DirectContent;
                    cb17.BeginText();
                    cb17.SetFontAndSize(baseFont, 10.0f);
                    cb17.ShowTextAligned(Element.ALIGN_LEFT, Convert.ToDateTime(listRpt.PackDate).ToString(VarGlobals.FormatD), 182f, 55f, 0);
                    cb17.EndText();

                    PdfContentByte cb18 = writer.DirectContent;
                    cb18.BeginText();
                    cb18.SetFontAndSize(baseFont, 10.0f);
                    cb18.ShowTextAligned(Element.ALIGN_LEFT, listRpt.ReservationNo.ToString(), 70f, 35f, 0);
                    cb18.EndText();

                    PdfContentByte cb19 = writer.DirectContent;
                    cb19.BeginText();
                    cb19.SetFontAndSize(baseFont, 10.0f);
                    cb19.ShowTextAligned(Element.ALIGN_LEFT, listRpt.CreateBy.ToString(),60f, 15f, 0);
                    cb19.EndText();

                    ///----------------------------------------------------------------
                    #endregion

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


        }

        #endregion
    }
}
