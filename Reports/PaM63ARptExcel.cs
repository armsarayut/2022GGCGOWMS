using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Public;

namespace GoWMS.Server.Reports
{
    public class PaM63ARptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<Class6_3_A> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.3.1");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "5.3.1.Inventory location" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "BATCH";
                worksheet.Cell(rptRows, 2).Value = "SKU";
                worksheet.Cell(rptRows, 3).Value = "NAME";
                worksheet.Cell(rptRows, 4).Value = "QTY";
                worksheet.Cell(rptRows, 5).Value = "PALLET NO";
                worksheet.Cell(rptRows, 6).Value = "TAG";
                worksheet.Cell(rptRows, 7).Value = "LOCATION";

                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + rpt.Batch_Number;
                    worksheet.Cell(rptRows, 2).Value = "'" + rpt.Item_Code;
                    worksheet.Cell(rptRows, 3).Value = "'" + rpt.Item_Name;
                    worksheet.Cell(rptRows, 4).Value = "'" + string.Format(VarGlobals.FormatN3, rpt.Qty);
                    worksheet.Cell(rptRows, 5).Value = "'" + rpt.Palletgo;
                    worksheet.Cell(rptRows, 6).Value = "'" + rpt.Su_No;
                    worksheet.Cell(rptRows, 7).Value = "'" + rpt.Shelfname;
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
