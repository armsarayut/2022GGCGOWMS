using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Inb;

namespace GoWMS.Server.Reports
{
    public class IbpGRNRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<Inb_Goodreceipt_Go> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("1.1");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "1.1.Receiving Order" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "DNDate";
                worksheet.Cell(rptRows, 2).Value = "DNNo";
                worksheet.Cell(rptRows, 3).Value = "DNSeq";
                worksheet.Cell(rptRows, 4).Value = "ItemCode";
                worksheet.Cell(rptRows, 5).Value = "ItemName";
                worksheet.Cell(rptRows, 6).Value = "Qty";
                worksheet.Cell(rptRows, 7).Value = "Unit";
                worksheet.Cell(rptRows, 8).Value = "LotNo";
                worksheet.Cell(rptRows, 9).Value = "BatchNo";

                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = rpt.Docdate;
                    worksheet.Cell(rptRows, 2).Value = rpt.Pono;
                    worksheet.Cell(rptRows, 3).Value = rpt.Pallettag;
                    worksheet.Cell(rptRows, 4).Value = rpt.Itemcode;
                    worksheet.Cell(rptRows, 5).Value = rpt.Itemname;
                    worksheet.Cell(rptRows, 6).Value = rpt.Quantity;
                    worksheet.Cell(rptRows, 7).Value = rpt.Unit;
                    worksheet.Cell(rptRows, 8).Value = rpt.Docno;
                    worksheet.Cell(rptRows, 9).Value = rpt.Docnote;
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
