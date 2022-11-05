using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Inv;

namespace GoWMS.Server.Reports
{
    public class WhStockbyCustomerRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Vrpt_shelf_listInfo> ListReport = new List<Vrpt_shelf_listInfo>();
        public byte[] Report(List<InvStockSumByCus> ListRpt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.3.4");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "5.3.4.Inventory Customer" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "ITEMCODE";
                worksheet.Cell(rptRows, 2).Value = "ITEMNAME";
                worksheet.Cell(rptRows, 3).Value = "CUSTOMER";
                worksheet.Cell(rptRows, 4).Value = "PALLET";
                worksheet.Cell(rptRows, 5).Value = "TOTALSTOCK";
                worksheet.Cell(rptRows, 6).Value = "LOCATION";
                worksheet.Cell(rptRows, 7).Value = "LANE";
                worksheet.Cell(rptRows, 8).Value = "BANK";
                worksheet.Cell(rptRows, 9).Value = "BAY";
                worksheet.Cell(rptRows, 10).Value = "LEVEL";
               

                foreach (var rpt in ListRpt)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = rpt.Itemcode;
                    worksheet.Cell(rptRows, 2).Value = rpt.Itemname;
                    worksheet.Cell(rptRows, 3).Value = rpt.Cusname;
                    worksheet.Cell(rptRows, 4).Value = rpt.Pallteno;
                    worksheet.Cell(rptRows, 5).Value = rpt.Totalstock;
                    worksheet.Cell(rptRows, 6).Value = "'" + rpt.Storagebin;
                    worksheet.Cell(rptRows, 7).Value = rpt.StorageLane;
                    worksheet.Cell(rptRows, 8).Value = rpt.StorageBank;
                    worksheet.Cell(rptRows, 9).Value = rpt.StorageBay;
                    worksheet.Cell(rptRows, 10).Value = rpt.StorageLevel;
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
