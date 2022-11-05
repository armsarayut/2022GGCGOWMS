using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Oub;

namespace GoWMS.Server.Reports
{
    public class OubPicklistRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<Sap_Storeout> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("1.3");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "3.3.Queue Picking" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "QUEUEDATE";
                worksheet.Cell(rptRows, 2).Value = "SONO";
                worksheet.Cell(rptRows, 3).Value = "REQUESTDATE";
                worksheet.Cell(rptRows, 4).Value = "ITEMCODE";
                worksheet.Cell(rptRows, 5).Value = "ITEMNAME";
                worksheet.Cell(rptRows, 6).Value = "PALLET";
                worksheet.Cell(rptRows, 7).Value = "DNSEQ";
                worksheet.Cell(rptRows, 8).Value = "QTY";
                worksheet.Cell(rptRows, 9).Value = "UNIT";



                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = Convert.ToDateTime(rpt.Created).ToString(VarGlobals.FormatDT);
                    worksheet.Cell(rptRows, 2).Value = rpt.Order_No;
                    worksheet.Cell(rptRows, 3).Value = Convert.ToDateTime(rpt.Delivery_Date).ToString(VarGlobals.FormatD);
                    worksheet.Cell(rptRows, 4).Value = rpt.Item_Code;
                    worksheet.Cell(rptRows, 5).Value =  rpt.Item_Name;
                    worksheet.Cell(rptRows, 6).Value = rpt.Pallet_No;
                    worksheet.Cell(rptRows, 7).Value = rpt.Su_No;
                    worksheet.Cell(rptRows, 8).Value = string.Format(VarGlobals.FormatN0, rpt.Request_Qty);
                    worksheet.Cell(rptRows, 9).Value = rpt.Unit;

                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
