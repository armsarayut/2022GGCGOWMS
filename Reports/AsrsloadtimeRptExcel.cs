using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Wcs;

namespace GoWMS.Server.Reports
{
    public class AsrsloadtimeRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        List<AsrsLoadtime> _ListRpt = new List<AsrsLoadtime>();
        public byte[] Report(List<AsrsLoadtime> ListRpt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.5.6");
                int startRows = 4;
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);

                worksheet.Cell("B1").Value = "5.5.6.ASRS-Loadtime" + " Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "PALLET";
                worksheet.Cell(rptRows, 2).Value = "TASKCODE";
                worksheet.Cell(rptRows, 3).Value = "TASKTYPE";
                worksheet.Cell(rptRows, 4).Value = "SRM";
                worksheet.Cell(rptRows, 5).Value = "SOURCE";
                worksheet.Cell(rptRows, 6).Value = "DESTINATION";
                worksheet.Cell(rptRows, 7).Value = "START";
                worksheet.Cell(rptRows, 8).Value = "END";
                worksheet.Cell(rptRows, 9).Value = "TIME";

                foreach (var rpt in ListRpt)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = rpt.Lpncode;
                    worksheet.Cell(rptRows, 2).Value = rpt.Work_code;
                    worksheet.Cell(rptRows, 3).Value = rpt.Work_text_th;
                    worksheet.Cell(rptRows, 4).Value = string.Format(VarGlobals.FormatD2, rpt.Srm_no);
                    worksheet.Cell(rptRows, 5).Value = string.Format(VarGlobals.FormatD9, rpt.Srm_from);
                    worksheet.Cell(rptRows, 6).Value = string.Format(VarGlobals.FormatD9, rpt.Srm_to);
                    worksheet.Cell(rptRows, 7).Value = Convert.ToDateTime(rpt.Stime).ToString(VarGlobals.FormatDT);
                    worksheet.Cell(rptRows, 8).Value = Convert.ToDateTime(rpt.Etime).ToString(VarGlobals.FormatDT);
                    worksheet.Cell(rptRows, 9).Value = rpt.Loadtime;
                }
                #endregion

                worksheet.SheetView.Freeze(startRows, 1);
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
