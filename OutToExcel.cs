using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Den.Common
{
    /// <summary>
    /// 无Excel组件导出Excel库
    /// </summary>
   public  class OutToExcel
    {
       #region 使用
       //private void btnExport_Click(object sender, System.EventArgs e)
       //{
       //    StringBuilder OutFileContent = new StringBuilder();//容器
       //    //写头文件
       //    OutFileContent = AddHeadFile(OutFileContent);
       //    //写内容
       //    StringBuilder sbMsg = new StringBuilder();//容器
       //    OutFileContent.Append(this.AddContentFile(sbMsg, ds));
       //    //写尾文件
       //    OutFileContent = AddEndFile(OutFileContent);
       //    //保存到xls
       //    string strRandomFileName = "GraduateApplyQuery";
       //    string strPath = Server.MapPath(Context.Request.ApplicationPath) + "\\ExcelReport\\Report\\";
       //    string strExcelFile = strPath + strRandomFileName + ".xls";
       //    FileStream OutFile = new FileStream(strExcelFile, FileMode.Create, FileAccess.Write);
       //    byte[] btArray = new byte[OutFileContent.Length];
       //    btArray = Encoding.UTF8.GetBytes(OutFileContent.ToString());
       //    OutFile.Write(btArray, 0, btArray.Length);
       //    OutFile.Flush();
       //    OutFile.Close();

       //    WriteThreadLog("写文件 => 3");
       //    Response.Redirect(UrlBase + "/ExcelReport/Report/" + strRandomFileName + ".xls");
       //}
       #endregion
       #region 写Excel头
       /// <summary>
        ///写Excel头
        /// </summary>
        /// <param name="OutFileContent"></param>
        /// <returns></returns>
        public static StringBuilder AddHeadFile(StringBuilder OutFileContent)
        {
            OutFileContent.Append("<?xml version=\"1.0\"?>\r\n");
            OutFileContent.Append("<?mso-application progid=\"Excel.Sheet\"?>\r\n");
            OutFileContent.Append("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
            OutFileContent.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n");
            OutFileContent.Append(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"\r\n");
            OutFileContent.Append(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
            OutFileContent.Append(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">\r\n");
            OutFileContent.Append(" <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">\r\n");
            OutFileContent.Append("  <Author>panss</Author>\r\n");
            OutFileContent.Append("  <LastAuthor>Оґ¶ЁТе</LastAuthor>\r\n");
            OutFileContent.Append("  <Created>2004-12-31T03:40:31Z</Created>\r\n");
            OutFileContent.Append("  <Company>Prcedu</Company>\r\n");
            OutFileContent.Append("  <Version>12.00</Version>\r\n");
            OutFileContent.Append(" </DocumentProperties>\r\n");
            OutFileContent.Append(" <OfficeDocumentSettings xmlns=\"urn:schemas-microsoft-com:office:office\">\r\n");
            OutFileContent.Append("  <DownloadComponents/>\r\n");
            OutFileContent.Append("  <LocationOfComponents HRef=\"file:///F:\\Tools\\OfficeXP\\OfficeXP\\\"/>\r\n");
            OutFileContent.Append(" </OfficeDocumentSettings>\r\n");
            OutFileContent.Append(" <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">\r\n");
            OutFileContent.Append("  <WindowHeight>9000</WindowHeight>\r\n");
            OutFileContent.Append("  <WindowWidth>10620</WindowWidth>\r\n");
            OutFileContent.Append("  <WindowTopX>480</WindowTopX>\r\n");
            OutFileContent.Append("  <WindowTopY>45</WindowTopY>\r\n");
            OutFileContent.Append("  <ProtectStructure>False</ProtectStructure>\r\n");
            OutFileContent.Append("  <ProtectWindows>False</ProtectWindows>\r\n");
            OutFileContent.Append(" </ExcelWorkbook>\r\n");
            OutFileContent.Append(" <Styles>\r\n");
            OutFileContent.Append("  <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n");
            OutFileContent.Append("   <Alignment ss:Vertical=\"Center\" />\r\n");
            OutFileContent.Append("   <Borders/>\r\n");
            OutFileContent.Append("   <Font ss:FontName=\"ЛОМе\" x:CharSet=\"134\" ss:Size=\"12\"/>\r\n");
            OutFileContent.Append("   <Interior/>\r\n");
            OutFileContent.Append("   <NumberFormat/>\r\n");
            OutFileContent.Append("   <Protection/>\r\n");
            OutFileContent.Append("  </Style>\r\n");
            OutFileContent.Append("  <Style ss:ID=\"s62\">\r\n");
            OutFileContent.Append("   <Alignment ss:Vertical=\"Center\" ss:Horizontal=\"Center\" ss:WrapText=\"1\"/>\r\n");
            OutFileContent.Append("   <Font ss:FontName=\"ЛОМе\" x:CharSet=\"134\" ss:Size=\"9\"/>\r\n");
            OutFileContent.Append("  </Style>\r\n");
            OutFileContent.Append("  <Style ss:ID=\"s74\">\r\n");
            OutFileContent.Append("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>\r\n");
            OutFileContent.Append("   <Borders>\r\n");
            OutFileContent.Append("  <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\r\n");
            OutFileContent.Append("  <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\r\n");
            OutFileContent.Append("  <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\r\n");
            OutFileContent.Append("  <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\r\n");
            OutFileContent.Append("  </Borders>\r\n");
            OutFileContent.Append("   <Font ss:FontName=\"ЛОМе\" x:CharSet=\"134\" ss:Size=\"12\" ss:Bold=\"1\"/>\r\n");
            OutFileContent.Append("   <Interior ss:Color=\"#BFBFBF\" ss:Pattern=\"Solid\"/>\r\n");
            OutFileContent.Append("  </Style>\r\n");
            OutFileContent.Append(" </Styles>\r\n");
            OutFileContent.Append(" <Worksheet ss:Name=\"Sheet1\">\r\n");
            OutFileContent.Append("  <Table ss:ExpandedColumnCount=\"255\" x:FullColumns=\"1\" \r\n");
            OutFileContent.Append("x:FullRows=\"1\" ss:StyleID=\"s62\" ss:DefaultColumnWidth=\"75\" ss:DefaultRowHeight=\"20.25\">\r\n");
            OutFileContent.Append("<Column ss:StyleID=\"s62\" ss:AutoFitWidth=\"0\" ss:Width=\"112.5\"/>\r\n");
            return OutFileContent;
        }
        #endregion

        #region 写数据内容
        //private StringBuilder AddContentFile(StringBuilder OutFileContent,DataSet ds)
        //{

        //    //写列头
        //            OutFileContent.Append("<Row ss:AutoFitHeight=\"0\">");
        //            OutFileContent.Append("<Cell><Data ss:Type=\"String\">姓名</Data></Cell>");
        //            OutFileContent.Append("<Cell><Data ss:Type=\"String\">数学</Data></Cell>");
        //            OutFileContent.Append("<Cell><Data ss:Type=\"String\">语文</Data></Cell>");
        //            OutFileContent.Append("<Cell><Data ss:Type=\"String\">英语</Data></Cell>");
        //            OutFileContent.Append("</Row>");
        //            //写内容
        //            foreach(DataRow row in ds.Tables[0].Rows)
        //            {
        //                OutFileContent.Append("<Row ss:AutoFitHeight=\"0\">");
        //                OutFileContent.Append("<Cell><Data ss:Type=\"String\">"+row["GraduateBatchName"].ToString()+"</Data></Cell>"); 
        //                OutFileContent.Append("<Cell><Data  ss:Type=\"String\">"+row["studyRecruitBatchName"].ToString()+"</Data></Cell>"); 
        //                OutFileContent.Append("<Cell><Data ss:Type=\"String\">"+row["EnterRecruitBatchName"].ToString()+"</Data></Cell>");  
        //                OutFileContent.Append("<Cell><Data ss:Type=\"String\">"+row["StudyLevelName"].ToString()+"</Data></Cell>"); 
        //                OutFileContent.Append("</Row>");
        //            }
        //    return OutFileContent;
        //}
       #endregion

        #region 写Excel尾
        /// <summary>
        ///  写Excel尾
        /// </summary>
        /// <param name="OutFileContent"></param>
        /// <returns></returns>
        public static StringBuilder AddEndFile(StringBuilder OutFileContent)
        {
            OutFileContent.Append("</Table>\r\n");
            OutFileContent.Append("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">\r\n");
            OutFileContent.Append("<Unsynced/>\r\n");
            OutFileContent.Append("<Print>\r\n");
            OutFileContent.Append("    <ValidPrinterInfo/>\r\n");
            OutFileContent.Append("    <PaperSizeIndex>9</PaperSizeIndex>\r\n");
            OutFileContent.Append("    <HorizontalResolution>600</HorizontalResolution>\r\n");
            OutFileContent.Append("    <VerticalResolution>0</VerticalResolution>\r\n");
            OutFileContent.Append("</Print>\r\n");
            OutFileContent.Append("<Selected/>\r\n");
            OutFileContent.Append("<Panes>\r\n");
            OutFileContent.Append("    <Pane>\r\n");
            OutFileContent.Append("    <Number>3</Number>\r\n");
            OutFileContent.Append("    <RangeSelection>R1:R65536</RangeSelection>\r\n");
            OutFileContent.Append("    </Pane>\r\n");
            OutFileContent.Append("</Panes>\r\n");
            OutFileContent.Append("<ProtectObjects>False</ProtectObjects>\r\n");
            OutFileContent.Append("<ProtectScenarios>False</ProtectScenarios>\r\n");
            OutFileContent.Append("</WorksheetOptions>\r\n");
            OutFileContent.Append("</Worksheet>\r\n");
            OutFileContent.Append("<Worksheet ss:Name=\"Sheet2\">\r\n");
            OutFileContent.Append("<Table ss:ExpandedColumnCount=\"1\" ss:ExpandedRowCount=\"1\" x:FullColumns=\"1\"\r\n");
            OutFileContent.Append("x:FullRows=\"1\" ss:DefaultColumnWidth=\"54\" ss:DefaultRowHeight=\"14.25\">\r\n");
            OutFileContent.Append("<Row ss:AutoFitHeight=\"0\"/>\r\n");
            OutFileContent.Append("</Table>\r\n");
            OutFileContent.Append("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">\r\n");
            OutFileContent.Append("<Unsynced/>\r\n");
            OutFileContent.Append("<ProtectObjects>False</ProtectObjects>\r\n");
            OutFileContent.Append("<ProtectScenarios>False</ProtectScenarios>\r\n");
            OutFileContent.Append("</WorksheetOptions>\r\n");
            OutFileContent.Append("</Worksheet>\r\n");
            OutFileContent.Append("<Worksheet ss:Name=\"Sheet3\">\r\n");
            OutFileContent.Append("<Table ss:ExpandedColumnCount=\"1\" ss:ExpandedRowCount=\"1\" x:FullColumns=\"1\"\r\n");
            OutFileContent.Append("x:FullRows=\"1\" ss:DefaultColumnWidth=\"54\" ss:DefaultRowHeight=\"14.25\">\r\n");
            OutFileContent.Append("<Row ss:AutoFitHeight=\"0\"/>\r\n");
            OutFileContent.Append("</Table>\r\n");
            OutFileContent.Append("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">\r\n");
            OutFileContent.Append("<Unsynced/>\r\n");
            OutFileContent.Append("<ProtectObjects>False</ProtectObjects>\r\n");
            OutFileContent.Append("<ProtectScenarios>False</ProtectScenarios>\r\n");
            OutFileContent.Append("</WorksheetOptions>\r\n");
            OutFileContent.Append("</Worksheet>\r\n");
            OutFileContent.Append("</Workbook>\r\n");
            return OutFileContent;
        }
        #endregion
    }
      
}

