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
    public class IbpOrderQueueRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceive_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceive_Go>();
        public byte[] Report(List<Inb_Goodreceive_Go> Inb_Goodreceive_Go_s)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("1.2");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "1.2.Good Receive" + " Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "QueuDate";
                worksheet.Cell(rptRows, 2).Value = "Pallet";
                worksheet.Cell(rptRows, 3).Value = "DNNo";
                worksheet.Cell(rptRows, 4).Value = "DNSeq";
                worksheet.Cell(rptRows, 5).Value = "Itemcode";
                worksheet.Cell(rptRows, 6).Value = "ItemName";
                worksheet.Cell(rptRows, 7).Value = "Qty";
                worksheet.Cell(rptRows, 8).Value = "Unit";

                foreach (var rpt in Inb_Goodreceive_Go_s)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + rpt.Created;
                    worksheet.Cell(rptRows, 2).Value = "'" + rpt.Pallteno;
                    worksheet.Cell(rptRows, 3).Value = "'" + rpt.Pono;
                    worksheet.Cell(rptRows, 4).Value = "'" + rpt.Pallettag;
                    worksheet.Cell(rptRows, 5).Value = "'" + rpt.Itemcode;
                    worksheet.Cell(rptRows, 6).Value = "'" + rpt.Itemname;
                    worksheet.Cell(rptRows, 7).Value = "'" + rpt.Quantity;
                    worksheet.Cell(rptRows, 8).Value = "'" + rpt.Unit;
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
