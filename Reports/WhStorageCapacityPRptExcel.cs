using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models;

namespace GoWMS.Server.Reports
{
    public class WhStorageCapacityPRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Vrpt_shelf_listInfo> ListReport = new List<Vrpt_shelf_listInfo>();
        public byte[] Report(List<WhStorageCapacity> ListRpt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("2.2");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "2.2.Capacity" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "Lane";
                worksheet.Cell(rptRows, 2).Value = "Occupied";
                worksheet.Cell(rptRows, 3).Value = "Mass";
                worksheet.Cell(rptRows, 4).Value = "Block/Error";
                worksheet.Cell(rptRows, 5).Value = "ProhibitedLocation";
                worksheet.Cell(rptRows, 6).Value = "Total";
                worksheet.Cell(rptRows, 7).Value = "OccupancyRate";

                foreach (var rpt in ListRpt)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = rpt.Srmname;
                    worksheet.Cell(rptRows, 2).Value = rpt.Locavlt1;
                    worksheet.Cell(rptRows, 3).Value = rpt.Locemp;
                    worksheet.Cell(rptRows, 4).Value = rpt.Perr;
                    worksheet.Cell(rptRows, 5).Value = rpt.Prohloc;
                    worksheet.Cell(rptRows, 6).Value = rpt.Total;
                    worksheet.Cell(rptRows, 7).Value = rpt.OccRate;


                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
