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
    public class MasPrivilegeRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<UserPrivilege> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("7.1");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 40;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.25);
                image.ScaleHeight(.25);
                worksheet.Cell("B1").Value = "7.1.Privileges" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString(VarGlobals.FormatDT)}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "GROUP";
                worksheet.Cell(rptRows, 2).Value = "MENU";
                worksheet.Cell(rptRows, 3).Value = "ACCESS";
                worksheet.Cell(rptRows, 4).Value = "ADD";
                worksheet.Cell(rptRows, 5).Value = "EDIT";
                worksheet.Cell(rptRows, 6).Value = "DELETE";
                worksheet.Cell(rptRows, 7).Value = "REPORT";
                worksheet.Cell(rptRows, 8).Value = "APPROVE";

                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + rpt.Ugdesc;
                    worksheet.Cell(rptRows, 2).Value = "'" + rpt.Menu_desc;
                    worksheet.Cell(rptRows, 3).Value = "'" + rpt.Role_acc;
                    worksheet.Cell(rptRows, 4).Value = "'" + rpt.Role_add;
                    worksheet.Cell(rptRows, 5).Value = "'" + rpt.Role_edit;
                    worksheet.Cell(rptRows, 6).Value = "'" + rpt.Role_del;
                    worksheet.Cell(rptRows, 7).Value = "'" + rpt.Role_rpt;
                    worksheet.Cell(rptRows, 8).Value = "'" + rpt.Role_apv;
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
