
using Microsoft.Win32;
using StersTransport.PresentationModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
 

namespace StersTransport.Helpers
{
    public class exportHelper
    {

       
        public void Export_To_Excel(DataTable dgv,string outputfilename,bool freezeheaders,bool sorttableheaders)
        {
            CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
           
            Microsoft.Office.Interop.Excel._Workbook ExcelBook;
            
            Microsoft.Office.Interop.Excel._Worksheet ExcelSheet;

           


           //create object of excel
           ExcelBook = (Microsoft.Office.Interop.Excel._Workbook)ExcelApp.Workbooks.Add(1);
            ExcelSheet = (Microsoft.Office.Interop.Excel._Worksheet)ExcelBook.ActiveSheet;
            int i = 0;
            int j = 0;




            Microsoft.Office.Interop.Excel.Range rng1;
            //export header



            for (i = 1; i <= dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i - 1].ColumnName == "Sender_Tel" || dgv.Columns[i - 1].ColumnName == "Receiver_Tel"|| dgv.Columns[i - 1].ColumnName == "Sender_ID")
                { ExcelSheet.Columns[i].NumberFormat = "@"; }

            }

            for (i = 1; i <= dgv.Columns.Count; i++)
            {

                ExcelSheet.Cells[1, i] = dgv.Columns[i - 1].ColumnName;

                // ExcelSheet.Range[1, i].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;


                // borders 
               // rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, i], ExcelSheet.Cells[1, i]].Cells;
              //  BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));

            }

            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, 1], ExcelSheet.Cells[1, dgv.Columns.Count]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;
            rng.Font.Bold = true;



            //export data
            string formattedvalue = string.Empty;
            for (i = 1; i <= dgv.Rows.Count; i++)
            {

                for (j = 1; j <= dgv.Columns.Count; j++)
                {
                    
                    // check if column type is double 
                    if (dgv.Columns[j - 1].DataType == typeof(double))
                    {
                        if (dgv.Rows[i - 1][j - 1] != DBNull.Value)
                        {
                            formattedvalue = string.Format("{0:#,0.##}", Convert.ToDouble(dgv.Rows[i - 1][j - 1]));
                        }
                        else
                        {
                            formattedvalue = string.Empty;
                        }

                    }
                    else
                    { formattedvalue = dgv.Rows[i - 1][j - 1].ToString(); }


                    //   ExcelSheet.Cells[i + 1, j] = dgv.Rows[i - 1][j - 1];
                    ExcelSheet.Cells[i + 1, j] = formattedvalue;


                    // borders 
                    //  rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[i + 1, j], ExcelSheet.Cells[i + 1, j]].Cells;
                    //  rng1.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    // BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));

                }
            }





            ExcelSheet.UsedRange.Cells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            ExcelSheet.UsedRange.HorizontalAlignment= Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            ExcelSheet.UsedRange.Font.Bold = true;
            //autofit
            ExcelSheet.Columns.AutoFit();
          
            //freeze header
            if (freezeheaders)
            {
                ExcelSheet.Activate();
                ExcelSheet.Application.ActiveWindow.SplitRow = 1;
                ExcelSheet.Application.ActiveWindow.FreezePanes = true;
            }

            // header height 
            Microsoft.Office.Interop.Excel.Range HeaderRow = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Rows[1];
            HeaderRow.RowHeight = 33;
            HeaderRow.HorizontalAlignment= Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            HeaderRow.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            // sortable  headers
            //
            if (sorttableheaders)
            {
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            }

           // ExcelBook.DoNotPromptForConvert = true;
            ExcelBook.CheckCompatibility = false;
            ExcelBook.Application.DisplayAlerts = false;


            object misValue = System.Reflection.Missing.Value;
            ExcelBook.SaveAs(outputfilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //   ExcelApp.Visible = true;


            ExcelBook.Close(true, misValue, misValue);
            ExcelApp.Quit();

            Marshal.ReleaseComObject(ExcelSheet);
            Marshal.ReleaseComObject(ExcelBook);
            Marshal.ReleaseComObject(ExcelApp);
       
         //   ExcelSheet = null;
          //  ExcelBook = null;
           // ExcelApp = null;

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
        }

        public void exporttopdf(string header, DataTable dt,bool rorate_page,string FileName,float[]widthes)
        {
            if (dt.Rows.Count > 0)
            {
               
                try
                {


                   

                    string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Arial.ttf";
                    iTextSharp.text.pdf.BaseFont basefont = iTextSharp.text.pdf.BaseFont.CreateFont(fontpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font arabicFont = new iTextSharp.text.Font(basefont, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                    //  var el = new iTextSharp.text.Chunk();
                    iTextSharp.text.Font f2 = new iTextSharp.text.Font(basefont, 10);
                    iTextSharp.text.Font f2b = new iTextSharp.text.Font(basefont, 10, iTextSharp.text.Font.BOLD,iTextSharp.text.Color.WHITE);
                    iTextSharp.text.Font f3H = new iTextSharp.text.Font(basefont, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);
                    //  el.Font.Style, el.Font.Color);
                    //   el.Font = f2;


                    iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                   // pdfTable.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;
                    pdfTable.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;

                    //   string[] headers = getHeadersfromDatatable(dt);

                    //  pdfTable.SetWidths(GetHeaderWidths(f2b, headers));
                    pdfTable.SetWidths(widthes);
                    foreach (DataColumn column in dt.Columns)
                    {
                        iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell();

                        cell.AddElement(new iTextSharp.text.Paragraph(column.ColumnName, f2b) { Alignment= iTextSharp.text.Element.ALIGN_CENTER });
                        cell.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;

                        //  cell.BackgroundColor = iTextSharp.text.Color.;
                        cell.BackgroundColor = new iTextSharp.text.Color(35, 107, 201);
                         
                        pdfTable.AddCell(cell);
                    }

                    foreach (DataRow row in dt.Rows)
                    {

                        for (int c = 0; c < dt.Columns.Count; c++)
                        {
                            string formattedvalue = string.Empty;
                            // check if column type is double 
                            if (dt.Columns[c].DataType == typeof(double))
                            {
                                if (row[c] != DBNull.Value)
                                { formattedvalue = string.Format("{0:#,0.##}", (double)row[c]); }
                                else
                                { formattedvalue = string.Empty; }
                               
                            }
                            else
                            { formattedvalue = row[c].ToString(); }
                            iTextSharp.text.pdf.PdfPCell cell2 = new iTextSharp.text.pdf.PdfPCell();
                            cell2.AddElement(new iTextSharp.text.Paragraph(formattedvalue, f2) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });
                            cell2.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;
                            cell2.HorizontalAlignment = iTextSharp.text.pdf.PdfCell.ALIGN_CENTER;
                            pdfTable.AddCell(cell2);
                        }


                    }

                    using (FileStream stream = new FileStream(FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 20f, 20f, 10f);
                        if (rorate_page)
                        { pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A3.Rotate(), 10f, 20f, 20f, 10f); }




                        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // header
                        /*
                        iTextSharp.text.Paragraph HeaderParagraph = new iTextSharp.text.Paragraph(header);
                        //  HeaderParagraph.Font = f3H;
                        //  HeaderParagraph.Alignment= iTextSharp.text.Element.ALIGN_CENTER;
                        HeaderParagraph.Alignment = 1;
                        pdfDoc.Add(HeaderParagraph);
                        */
                        //  HeaderParagraph.SpacingAfter = 40;
                        // Set gap between line paragraphs.



                        iTextSharp.text.pdf.PdfPTable pdfTableHeader = new iTextSharp.text.pdf.PdfPTable(1);
                        pdfTableHeader.DefaultCell.Padding = 3;
                        pdfTableHeader.WidthPercentage = 100;
                        pdfTable.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell();
                        cell.AddElement(new iTextSharp.text.Paragraph(header, f3H) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });
                        cell.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;
                        cell.Border = 0;
                        pdfTableHeader.AddCell(cell);
                        pdfTableHeader.SpacingAfter = 30;
                        pdfTableHeader.SpacingBefore = 40;
                        pdfDoc.Add(pdfTableHeader);
                       
                        // table
                        pdfDoc.Add(pdfTable);

                        pdfDoc.Close();
                        stream.Close();
                    }

                    WpfMessageBox.Show("", "Data Exported successfully", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("no data..");
            }
        }

        private string[] getHeadersfromDatatable(DataTable dt)
        {
            string[] columnNames = dt.Columns.Cast<DataColumn>()
                                  .Select(x => x.ColumnName)
                                  .ToArray();
            return columnNames;
        }


        //special export ....
        public void exportSummarytopdf(string header, DataTable dt, bool rorate_page, string FileName)
        {
            if (dt.Rows.Count > 0)
            {

                try
                {
                    string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Arial.ttf";
                    iTextSharp.text.pdf.BaseFont basefont = iTextSharp.text.pdf.BaseFont.CreateFont(fontpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font arabicFont = new iTextSharp.text.Font(basefont, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                    //  var el = new iTextSharp.text.Chunk();
                    iTextSharp.text.Font f2 = new iTextSharp.text.Font(basefont, 8);
                    iTextSharp.text.Font f2b = new iTextSharp.text.Font(basefont, 8, iTextSharp.text.Font.BOLD);
                    //  el.Font.Style, el.Font.Color);
                    //   el.Font = f2;


                    iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
                    pdfTable.DefaultCell.Padding = 3;

                    pdfTable.WidthPercentage = 100;


                    pdfTable.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_LTR;
                    pdfTable.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;

                    // headers
                    foreach (DataColumn column in dt.Columns)
                    {
                        iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell();
                        cell.AddElement(new iTextSharp.text.Paragraph(column.Caption, f2b) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });
                        cell.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;


                        // color issue ...
                        if (column.ColumnName == "Total_Pallets" || column.ColumnName == "Total_Boxes")
                        {
                            cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.LightSkyBlue);
                        }

                        else if (column.ColumnName == "Total_Packiging_cost_IQD" || column.ColumnName == "Total_Custome_Cost_Qomrk"
                            || column.ColumnName == "Total_POST_DoorToDoor_IQD"
                            || column.ColumnName == "Total_Sub_Post_Cost_IQD"
                            || column.ColumnName == "Total_Discount_Post_Cost_Send"
                            || column.ColumnName == "TotalCashIn"

                            )
                        {
                            cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.Yellow);
                        }



                        else if (column.ColumnName == "Total_Paid_To_Company")
                        {
                            cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.LightGreen);
                        }

                        if (column.Ordinal > 13) 
                        {
                            //F7BA00
                           // System.Windows.Media.SolidColorBrush GreenColor = (System.Windows.Media.SolidColorBrush)(new System.Windows.Media.BrushConverter().ConvertFrom("#F7BA00"));
                            cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.Orange);
                        }


                        pdfTable.AddCell(cell);
                    }

                    foreach (DataRow row in dt.Rows)
                    {

                        for (int c = 0; c < dt.Columns.Count; c++)
                        {

                            iTextSharp.text.pdf.PdfPCell cell2 = new iTextSharp.text.pdf.PdfPCell();
                            if (dt.Columns[c].DataType == typeof(System.Double))
                            { cell2.AddElement(new iTextSharp.text.Paragraph(String.Format("{0:#,0.####}", (double)row[c]), f2) { Alignment = iTextSharp.text.Element.ALIGN_CENTER }); }
                            else
                            { cell2.AddElement(new iTextSharp.text.Paragraph(row[c].ToString(), f2) { Alignment = iTextSharp.text.Element.ALIGN_CENTER }); }
                          
                            cell2.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;

                            DataColumn column = dt.Columns[c];
                            // color issue ...
                            if (column.ColumnName == "Total_Pallets" || column.ColumnName == "Total_Boxes")
                            {
                                cell2.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.LightSkyBlue);
                            }

                            else if (column.ColumnName == "Total_Packiging_cost_IQD" || column.ColumnName == "Total_Custome_Cost_Qomrk"
                                || column.ColumnName == "Total_POST_DoorToDoor_IQD"
                                || column.ColumnName == "Total_Sub_Post_Cost_IQD"
                                || column.ColumnName == "Total_Discount_Post_Cost_Send"
                                || column.ColumnName == "TotalCashIn"

                                )
                            {
                                cell2.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.Yellow);
                            }



                            else if (column.ColumnName == "Total_Paid_To_Company")
                            {
                                cell2.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.LightGreen);
                            }

                            if (column.Ordinal > 13)
                            {
                                //F7BA00
                                // System.Windows.Media.SolidColorBrush GreenColor = (System.Windows.Media.SolidColorBrush)(new System.Windows.Media.BrushConverter().ConvertFrom("#F7BA00"));
                                cell2.BackgroundColor = new iTextSharp.text.Color(System.Drawing.Color.Orange);
                            }



                            pdfTable.AddCell(cell2);
                        }

                    }

                    using (FileStream stream = new FileStream(FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 20f, 20f, 10f);
                        if (rorate_page)
                        { pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 10f, 20f, 20f, 10f); }




                        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // header
                        iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(header);
                        p.Alignment = 1;
                    
                        pdfDoc.Add(p);

                        pdfDoc.Add(pdfTable);

                        pdfDoc.Close();
                        stream.Close();
                    }

                    WpfMessageBox.Show("", "Data Exported successfully", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("no data..");
            }
        }


        public void Export_Summary_To_Excel(DataTable dgv, string outputfilename, bool freezeheaders, bool sorttableheaders,bool addTwoEmptyrows)
        {
            CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel._Workbook ExcelBook;

            Microsoft.Office.Interop.Excel._Worksheet ExcelSheet;




            //create object of excel
            ExcelBook = (Microsoft.Office.Interop.Excel._Workbook)ExcelApp.Workbooks.Add(1);
            ExcelSheet = (Microsoft.Office.Interop.Excel._Worksheet)ExcelBook.ActiveSheet;
            int i = 0;
            int j = 0;
            //export header

            for (i = 1; i <= dgv.Columns.Count; i++)
            {
                ExcelSheet.Columns[i].NumberFormat = "@";

            }

            for (i = 1; i <= dgv.Columns.Count; i++)
            {
                ExcelSheet.Cells[1, i] = dgv.Columns[i - 1].Caption;

                // ExcelSheet.Range[1, i].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;
                // borders 
             //   Microsoft.Office.Interop.Excel.Range rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, i], ExcelSheet.Cells[1, i]].Cells;
              //  BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));
            }

            // bold

            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, 1], ExcelSheet.Cells[1, dgv.Columns.Count]].Cells;
           // rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;
            rng.Font.Bold = true;

            #region Colors
            // colors 
            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, 3], ExcelSheet.Cells[1, 4]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;


            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, 8], ExcelSheet.Cells[1, 13]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;



            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, 14], ExcelSheet.Cells[1, 14]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGreen;

            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[1, 15], ExcelSheet.Cells[1, dgv.Columns.Count]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbOrange;

            //

            // colors 
            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[2, 3], ExcelSheet.Cells[2, 4]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;


            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[2, 8], ExcelSheet.Cells[2, 13]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;



            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[2, 14], ExcelSheet.Cells[2, 14]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGreen;

            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[2, 15], ExcelSheet.Cells[1, dgv.Columns.Count]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbOrange;

            int lastrowIndex = 3;
            if (addTwoEmptyrows)
            {
                lastrowIndex = 5;
            }


            // last row clors 
            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[lastrowIndex, 3], ExcelSheet.Cells[lastrowIndex, 4]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;


            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[lastrowIndex, 8], ExcelSheet.Cells[lastrowIndex, 13]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;



            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[lastrowIndex, 14], ExcelSheet.Cells[lastrowIndex, 14]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGreen;

            rng = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[lastrowIndex, 15], ExcelSheet.Cells[lastrowIndex, dgv.Columns.Count]].Cells;
            rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbOrange;

            #endregion

            //export data
            string formattedvalue = string.Empty;
           
            for (i = 1; i <= dgv.Rows.Count; i++)
            {
                if (i == dgv.Rows.Count)// if the last row
                {
                    if (addTwoEmptyrows)
                    {
                        int emptyrowstoaddExitCounter = 3;
                        for (int kk = 1; kk <= emptyrowstoaddExitCounter; kk++)
                        {
                            for (j = 1; j <= dgv.Columns.Count; j++)
                            {
                                ExcelSheet.Cells[i + kk, j] = string.Empty;
                              //  Microsoft.Office.Interop.Excel.Range rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[i + kk, j], ExcelSheet.Cells[i + kk, j]].Cells;
                              //  BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));
                            }
                        }

                        // add last row
                        for (j = 1; j <= dgv.Columns.Count; j++)
                        {
                            // check if column type is double 
                            if (dgv.Columns[j - 1].DataType == typeof(double))
                            {
                                if (dgv.Rows[i - 1][j - 1] != DBNull.Value)
                                {
                                    formattedvalue = string.Format("{0:#,0.##}", Convert.ToDouble(dgv.Rows[i - 1][j - 1]));
                                }
                                else
                                {
                                    formattedvalue = string.Empty;
                                }

                            }
                            else
                            { formattedvalue = dgv.Rows[i - 1][j - 1].ToString(); }

                            ExcelSheet.Cells[i + emptyrowstoaddExitCounter, j] = formattedvalue;
                            //   ExcelSheet.Cells[i + emptyrowstoaddExitCounter, j] = dgv.Rows[i - 1][j - 1];




                            //  Microsoft.Office.Interop.Excel.Range rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[i + emptyrowstoaddExitCounter, j], ExcelSheet.Cells[i + emptyrowstoaddExitCounter, j]].Cells;
                            //   BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));
                        }
                           

                    }
                    else
                    {
                        for (j = 1; j <= dgv.Columns.Count; j++)
                        {

                            if (dgv.Columns[j - 1].DataType == typeof(double))
                            {
                                if (dgv.Rows[i - 1][j - 1] != DBNull.Value)
                                {
                                    formattedvalue = string.Format("{0:#,0.##}", Convert.ToDouble(dgv.Rows[i - 1][j - 1]));
                                }
                                else
                                {
                                    formattedvalue = string.Empty;
                                }

                            }
                            else
                            { formattedvalue = dgv.Rows[i - 1][j - 1].ToString(); }

                            ExcelSheet.Cells[i + 1, j] = formattedvalue;
                          //  ExcelSheet.Cells[i + 1, j] = dgv.Rows[i - 1][j - 1];
                          //  Microsoft.Office.Interop.Excel.Range rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[i + 1, j], ExcelSheet.Cells[i + 1, j]].Cells;
                          //  BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));
                        }
                           
                    }
                }
                else
                {
                    for (j = 1; j <= dgv.Columns.Count; j++)
                    {
                        if (dgv.Columns[j - 1].DataType == typeof(double))
                        {
                            if (dgv.Rows[i - 1][j - 1] != DBNull.Value)
                            {
                                formattedvalue = string.Format("{0:#,0.##}", Convert.ToDouble(dgv.Rows[i - 1][j - 1]));
                            }
                            else
                            {
                                formattedvalue = string.Empty;
                            }

                        }
                        else
                        { formattedvalue = dgv.Rows[i - 1][j - 1].ToString(); }

                        ExcelSheet.Cells[i + 1, j] = formattedvalue;
                      //  ExcelSheet.Cells[i + 1, j] = dgv.Rows[i - 1][j - 1];


                        //  Microsoft.Office.Interop.Excel.Range rng1 = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Range[ExcelSheet.Cells[i + 1, j], ExcelSheet.Cells[i + 1, j]].Cells;
                        // BorderAround(rng1.Cells, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 0)));
                    }
                }
                
            }




            //autofit
            ExcelSheet.Columns.AutoFit();
            ExcelSheet.UsedRange.Cells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            ExcelSheet.UsedRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            ExcelSheet.UsedRange.Font.Bold = true;

            //freeze header
            if (freezeheaders)
            {
                ExcelSheet.Activate();
                ExcelSheet.Application.ActiveWindow.SplitRow = 1;
                ExcelSheet.Application.ActiveWindow.FreezePanes = true;
            }

            // header height 
            Microsoft.Office.Interop.Excel.Range HeaderRow = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Rows[1];
            HeaderRow.RowHeight = 33;
            HeaderRow.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            HeaderRow.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            // sortable  headers
            //
            if (sorttableheaders)
            {
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            }

            // ExcelBook.DoNotPromptForConvert = true;
            ExcelBook.CheckCompatibility = false;
            ExcelBook.Application.DisplayAlerts = false;


            object misValue = System.Reflection.Missing.Value;
            ExcelBook.SaveAs(outputfilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //   ExcelApp.Visible = true;


            ExcelBook.Close(true, misValue, misValue);
            ExcelApp.Quit();

            Marshal.ReleaseComObject(ExcelSheet);
            Marshal.ReleaseComObject(ExcelBook);
            Marshal.ReleaseComObject(ExcelApp);

            //   ExcelSheet = null;
            //  ExcelBook = null;
            // ExcelApp = null;

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
        }



        private void BorderAround(Microsoft.Office.Interop.Excel.Range range, int colour)
        {
            Microsoft.Office.Interop.Excel.Borders borders = range.Borders;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders.Color = colour;
         //   borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
         //   borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
         //   borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
         //   borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            borders = null;
        }


        public float[] GetHeaderWidths(iTextSharp.text.Font font, params string[] headers)
        {
            var total = 0;
            var columns = headers.Length;
            var widths = new int[columns];
            for (var i = 0; i < columns; ++i)
            {
                var w = font.GetCalculatedBaseFont(true).GetWidth(headers[i]);
                total += w;
                widths[i] = w;
            }
            var result = new float[columns];
            for (var i = 0; i < columns; ++i)
            {
                result[i] = (float)widths[i] / total * 100;
            }
            return result;
        }

    }
}
