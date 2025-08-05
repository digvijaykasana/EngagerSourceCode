using Aspose.Cells;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.DocumentProcessor
{
    public class ExcelProcessor<E>
    {

        public string ExportToExcel(List<E> collections, string savePath, string fileName, bool isXlsx = false, List<CustomTitle> titleList = null)
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string fullFilePath = savePath + fileName + (isXlsx == true ? ".xlsx" : ".xls");

            Workbook workBook = new Workbook();

            if(titleList == null)
            {
                WriteHeaderLine(workBook);

                WriteDataLine(collections, workBook);
            }
            else
            {
                WriteCustomTitleLine(workBook, titleList);
                WriteCustomHeaderLine(workBook, 1);
                WriteCustomDataLine(collections, workBook, 2);
            }

            workBook.Save(fullFilePath);

            return fullFilePath;
        }

        private void WriteCustomTitleLine(Workbook workBook, List<CustomTitle> titleList)
        {
            int row = 0;
            int column = 0;

            foreach (CustomTitle customTitle in titleList)
            {
                column = customTitle.ColumnNo;

                workBook.Worksheets[0].Cells[row, column].PutValue(customTitle.Value.ToUpper());
            }
        }

        private void WriteHeaderLine(Workbook workBook)
        {
            int column = 0;

            foreach(MemberInfo member in typeof(E).GetProperties())
            {
                object[] attrs = member.GetCustomAttributes(true);
                foreach(object attr in attrs)
                {
                    DisplayAttribute displayAttr = attr as DisplayAttribute;
                    if(displayAttr!=null)
                    {
                        workBook.Worksheets[0].Cells[0, column++].PutValue(displayAttr.Name);
                    }
                }
            }
        }

        private void WriteCustomHeaderLine(Workbook workBook, int startingRow)
        {
            int column = 0;

            foreach (MemberInfo member in typeof(E).GetProperties())
            {
                object[] attrs = member.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    DisplayAttribute displayAttr = attr as DisplayAttribute;
                    if (displayAttr != null)
                    {
                        workBook.Worksheets[0].Cells[startingRow, column++].PutValue(displayAttr.Name);
                    }
                }
            }
        }

        private void WriteCustomDataLine(List<E> collections, Workbook workBook, int startingRow)
        {
            int row = startingRow;
            int column = 0;

            foreach (E line in collections)
            {
                column = 0;
                foreach (MemberInfo member in typeof(E).GetProperties())
                {
                    object[] attrs = member.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        DisplayAttribute disAttr = attr as DisplayAttribute;
                        if (disAttr != null)
                        {
                            workBook.Worksheets[0].Cells[row, column++].PutValue(GetPropertyValue(line, member.Name));
                        }
                    }
                }
                row++;
            }
        }

        private void WriteDataLine(List<E> collections, Workbook workBook)
        {
            int row = 1;
            int column = 0;

            foreach(E line in collections)
            {
                column = 0;
                foreach(MemberInfo member in typeof(E).GetProperties())
                {
                    object[] attrs = member.GetCustomAttributes(true);
                    foreach(object attr in attrs)
                    {
                        DisplayAttribute disAttr = attr as DisplayAttribute;
                        if(disAttr!=null)
                        {
                            workBook.Worksheets[0].Cells[row, column++].PutValue(GetPropertyValue(line, member.Name));
                        }
                    }
                }
                row++;
            }
        }

        private object GetPropertyValue(object src, string propName)
        {
            try
            {
                return src.GetType().GetProperty(propName).GetValue(src, null);
            }
            catch
            {
                return null;
            }
        }


        public List<E> ImportFromExcel(string fullFilePath)
        {
            Workbook workBook = new Workbook(fullFilePath);

            Worksheet workSheet = workBook.Worksheets[0];

            int numberOfRow = workSheet.Cells.Rows.Count;
            int numberOfColumn = 0;
            if (numberOfRow != 0)
                numberOfColumn = workSheet.Cells.Count / numberOfRow;
            List<string> properties = new List<string>();

            List<E> list = new List<E>();

            for(int column1=0;column1<numberOfColumn;column1++)
            {
                properties.Add(workSheet.Cells[0, column1].StringValue);
            }

            for (int row = 1; row < numberOfRow; row++)
            {
                E obj = Activator.CreateInstance<E>();
                for (int column = 0; column < numberOfColumn; column++)
                {
                    try
                    {
                        var type = obj.GetType().GetProperty(properties[column].Trim()).PropertyType;
                        if(type == typeof(string))
                        {
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, workSheet.Cells[row, column].StringValue);
                        }else if(type == typeof(decimal))
                        {
                            decimal decValue = 0;
                            Decimal.TryParse(workSheet.Cells[row, column].StringValue, out decValue);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, decValue);
                        }
                        else if(type == typeof(int))
                        {
                            int value = 0;
                            int.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                        else if(type == typeof(Int64))
                        {
                            Int64 value = 0;
                            Int64.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                        else if(type == typeof(DateTime))
                        {
                            DateTime time = new DateTime();
                            time = Util.ConvertStringToDateTime(workSheet.Cells[row, column].StringValue, DateConfig.CULTURE);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, time);
                        }
                        else if(type == typeof(bool))
                        {
                            bool value = false;
                            bool.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }else if(type == typeof(float))
                        {
                            float value = 0;
                            float.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }else if(type == typeof(double))
                        {
                            double value = 0;
                            double.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                    }
                    catch(Exception ex)
                    {
                        string message = ex.Message;
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public List<E> ImportFromExcel(Stream fullFilePath)
        {
            Workbook workBook = new Workbook(fullFilePath);

            Worksheet workSheet = workBook.Worksheets[0];

            int numberOfRow = workSheet.Cells.Rows.Count;
            int numberOfColumn = 0;
            if (numberOfRow != 0)
                numberOfColumn = workSheet.Cells.Count / numberOfRow;
            List<string> properties = new List<string>();

            List<E> list = new List<E>();

            for (int column1 = 0; column1 < numberOfColumn; column1++)
            {
                properties.Add(workSheet.Cells[0, column1].StringValue);
            }

            for (int row = 1; row < numberOfRow; row++)
            {
                E obj = Activator.CreateInstance<E>();
                for (int column = 0; column < numberOfColumn; column++)
                {
                    try
                    {
                        var type = obj.GetType().GetProperty(properties[column].Trim()).PropertyType;
                        if (type == typeof(string))
                        {
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, workSheet.Cells[row, column].StringValue);
                        }
                        else if (type == typeof(decimal))
                        {
                            decimal decValue = 0;
                            Decimal.TryParse(workSheet.Cells[row, column].StringValue, out decValue);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, decValue);
                        }
                        else if (type == typeof(int))
                        {
                            int value = 0;
                            int.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                        else if (type == typeof(Int64))
                        {
                            Int64 value = 0;
                            Int64.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                        else if (type == typeof(DateTime))
                        {
                            DateTime time = new DateTime();
                            time = Util.ConvertStringToDateTime(workSheet.Cells[row, column].StringValue, DateConfig.CULTURE);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, time);
                        }
                        else if (type == typeof(bool))
                        {
                            bool value = false;
                            bool.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                        else if (type == typeof(float))
                        {
                            float value = 0;
                            float.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                        else if (type == typeof(double))
                        {
                            double value = 0;
                            double.TryParse(workSheet.Cells[row, column].StringValue, out value);
                            obj.GetType().GetProperty(properties[column].Trim()).SetValue(obj, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public string GenerateInvoice(InvoiceReportViewModel report, string excelTemplatePath,string savePath)
        {
            Workbook workbook = new Workbook(excelTemplatePath);

            Worksheet sheet = workbook.Worksheets[0];

            #region Invoice Header

            //Adding Customer Name            
            Aspose.Cells.Cell customerName = sheet.Cells[2, 1];
            if(report.needsVesselNameInFrontCompanyName)
            {
                customerName.Value = "Master / Owner \"" + report.Vessel + "\" c/o " + report.Customer;
            }
            else
            {
                customerName.Value = report.Customer;
            }


            //Adding Customer Address
            Aspose.Cells.Cell customerAddress = sheet.Cells[3, 1];
            customerAddress.Value = report.Address;

            //Adding Invoice Number
            Aspose.Cells.Cell invoiceNo = sheet.Cells[2, 3];
            invoiceNo.Value = report.InvoiceNo;

            //Adding Invoice Date
            Aspose.Cells.Cell invoiceDate = sheet.Cells[4, 3];
            invoiceDate.Value = report.DateStr;

            #endregion

            #region Vessel Name

            //Adding Vessel Cell
            Aspose.Cells.Range vessel = sheet.Cells.CreateRange(7, 1, 1, 2);
            vessel.Merge();

            //Adding Cell Style

            ////Adding Border
            vessel.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);

            Style vesselStyle = new Style();

            ////Alignment
            vesselStyle.HorizontalAlignment = TextAlignmentType.Center;
            vesselStyle.VerticalAlignment = TextAlignmentType.Center;

            ////Font
            vesselStyle.Font.IsBold = true;
            vessel.SetStyle(vesselStyle);

            vessel.Value = "\"" + report.Vessel + "\"";

            #endregion

            #region Adding Invoicing Details

            int startIndex = 8;
            bool firstTime = true;

            foreach (var details in report.Details)
            {
                //If invoicing detail is a header
                if (details.IsHeader)
                {
                    //Adding blank row cell 
                    var mergeCell = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
                    mergeCell.Merge();

                    //Applying style to blank row cell

                    ////Adding Top Border to blank row if first invoice item
                    if (firstTime)
                    {
                        mergeCell.SetOutlineBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                        firstTime = false;
                    }

                    mergeCell.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    mergeCell.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);

                    //Adding Is Taxable column to blank row
                    var rightColumn = sheet.Cells[startIndex, 4];
                    var rightColumnStyle = rightColumn.GetStyle();
                    rightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    rightColumn.SetStyle(rightColumnStyle);

                    //Index increased for header cell row
                    startIndex++;

                    //Adding invoice detail date to header cell row
                    var detailsDate = sheet.Cells[startIndex, 0];
                    if(!details.IsDNNo)
                    {
                        detailsDate.Value = details.InvoiceDateStr;
                    }

                    //Adding header description cell
                    var headerDescription = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
                    headerDescription.Merge();
                    headerDescription.Value = details.WorkOrderNo;
                    headerDescription.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    headerDescription.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);

                    //Adding header isTaxable cell
                    var rightColumn2 = sheet.Cells[startIndex, 4];
                    var rightColumnStyle2 = rightColumn2.GetStyle();
                    rightColumnStyle2.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    rightColumn2.SetStyle(rightColumnStyle2);

                    startIndex++;
                }
                else
                {
                    //Adding invoice detail description cell
                    var item = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
                    item.Merge();
                    item.Value = details.DisplayDescription;
                    item.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    item.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);

                    //Adding invoice detail amount cell
                    var totalAmount = sheet.Cells[startIndex, 3];
                    var totalAmountStyle = totalAmount.GetStyle();
                    totalAmountStyle.Number = 2;
                    totalAmount.SetStyle(totalAmountStyle);
                    totalAmount.Value = details.TotalAmount;

                    //Adding invoice detail isTaxable cell
                    var rightColumn = sheet.Cells[startIndex, 4];
                    var rightColumnStyle = rightColumn.GetStyle();
                    rightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    rightColumn.SetStyle(rightColumnStyle);
                    if(details.IsTaxable && report.IsTaxInvoice)
                    {
                        rightColumn.Value = "*";
                    }

                    startIndex++;
                }

                //Adding additional invoice detail description cell
                if (!string.IsNullOrEmpty(details.Description))
                {
                    var description = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
                    description.Merge();
                    description.Value = details.Description;
                    description.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    description.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    startIndex++;
                }
            }

            //Adding Last Blank Row
            var lastBlockFirstColumn = sheet.Cells[startIndex, 0];
            var lastBlockFirstColumnStyle = lastBlockFirstColumn.GetStyle();
            lastBlockFirstColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockFirstColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockFirstColumn.SetStyle(lastBlockFirstColumnStyle);

            var lastBlockSecondColumn = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
            lastBlockSecondColumn.Merge();
            lastBlockSecondColumn.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockSecondColumn.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);

            var lastBlockThirdColumn = sheet.Cells.CreateRange(startIndex, 3, 1, 2);
            lastBlockThirdColumn.Merge();
            lastBlockThirdColumn.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockThirdColumn.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);

            #endregion

            #region Grand Total

            startIndex++;
            startIndex++;

            //Adding Grand Total Field

            if(!report.IsTaxInvoice)
            {
                //Adding Grand Total Label cell
                var subTotal = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var subTotalStyle = subTotal.GetStyle();
                subTotalStyle.Pattern = BackgroundType.Solid;
                subTotalStyle.ForegroundColor = System.Drawing.Color.LightGray;
                subTotalStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                subTotalStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                subTotalStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                subTotalStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                subTotal.SetStyle(subTotalStyle);

                //Setting Grand Total Label cell value
                subTotal.Value = "Grand Total";

                //Adding Grand Total value cell
                var subTotalValue = sheet.Cells[startIndex, 3];

                //Adding Grand Total value cell style
                var subTotalValueStyle = subTotalValue.GetStyle();
                subTotalValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                subTotalValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                subTotalValueStyle.Number = 2;
                subTotalValue.SetStyle(subTotalValueStyle);

                //Setting Grand Total cell value
                subTotalValue.Value = report.GetGrandTotalWithoutTax();

                //Adding Grand Total IsTaxable cell
                var lastRightColumn = sheet.Cells[startIndex, 4];
                var lastRightColumnStyle = lastRightColumn.GetStyle();
                lastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                lastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                lastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                lastRightColumn.SetStyle(lastRightColumnStyle);
            }
            else
            {
                #region Total Taxable Amount

                //Adding Total Taxable AmountLabel cell
                var totalTaxAmt = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var totalTaxAmtStyle = totalTaxAmt.GetStyle();
                totalTaxAmtStyle.Pattern = BackgroundType.Solid;
                totalTaxAmtStyle.ForegroundColor = System.Drawing.Color.LightGray;
                totalTaxAmtStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmt.SetStyle(totalTaxAmtStyle);

                //Setting Total Taxable AmountLabel cell value
                totalTaxAmt.Value = "Total Taxable Amount";

                //Adding Total Taxable Amount value cell
                var totalTaxAmtValue = sheet.Cells[startIndex, 3];

                //Adding Total Taxable Amount value cell style
                var totalTaxAmtValueStyle = totalTaxAmtValue.GetStyle();
                totalTaxAmtValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtValueStyle.Number = 2;
                totalTaxAmtValue.SetStyle(totalTaxAmtValueStyle);

                //Setting Total Taxable Amountcell value
                totalTaxAmtValue.Value = report.TotalAmount;

                //Adding Total Taxable Amount IsTaxable cell
                var totalTaxAmtlastRightColumn = sheet.Cells[startIndex, 4];
                var totalTaxAmtlastRightColumnStyle = totalTaxAmtlastRightColumn.GetStyle();
                totalTaxAmtlastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtlastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtlastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalTaxAmtlastRightColumn.SetStyle(totalTaxAmtlastRightColumnStyle);

                startIndex++;

                #endregion

                #region GST Amount

                //Adding Total Taxable AmountLabel cell
                var gst = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var gstStyle = gst.GetStyle();
                gstStyle.Pattern = BackgroundType.Solid;
                gstStyle.ForegroundColor = System.Drawing.Color.LightGray;
                gstStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gst.SetStyle(gstStyle);

                //Setting Total Taxable AmountLabel cell value
                gst.Value = "Add " + report.TaxDescription + " % GST on * " + report.TotalAmount;

                //Adding Total Taxable Amount value cell
                var gstValue = sheet.Cells[startIndex, 3];

                //Adding Total Taxable Amount value cell style
                var gstValueStyle = gstValue.GetStyle();
                gstValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstValueStyle.Number = 2;
                gstValue.SetStyle(gstValueStyle);

                //Setting Total Taxable Amountcell value
                gstValue.Value = report.TaxAmount;

                //Adding Total Taxable Amount IsTaxable cell
                var gstlastRightColumn = sheet.Cells[startIndex, 4];
                var gstlastRightColumnStyle = gstlastRightColumn.GetStyle();
                gstlastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstlastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstlastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstlastRightColumn.SetStyle(gstlastRightColumnStyle);

                startIndex++;

                #endregion

                #region Total Non Taxable Amount

                //Adding Total Taxable AmountLabel cell
                var totalNonTaxAmt = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var totalNonTaxAmtStyle = totalNonTaxAmt.GetStyle();
                totalNonTaxAmtStyle.Pattern = BackgroundType.Solid;
                totalNonTaxAmtStyle.ForegroundColor = System.Drawing.Color.LightGray;
                totalNonTaxAmtStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmt.SetStyle(totalNonTaxAmtStyle);

                //Setting Total Taxable AmountLabel cell value
                totalNonTaxAmt.Value = "Total Non Taxable Amount";

                //Adding Total Taxable Amount value cell
                var totalNonTaxAmtValue = sheet.Cells[startIndex, 3];

                //Adding Total Taxable Amount value cell style
                var totalNonTaxAmtValueStyle = totalNonTaxAmtValue.GetStyle();
                totalNonTaxAmtValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtValueStyle.Number = 2;
                totalNonTaxAmtValue.SetStyle(totalNonTaxAmtValueStyle);

                //Setting Total Taxable Amountcell value
                totalNonTaxAmtValue.Value = report.TotalNonTaxableAmount;

                //Adding Total Taxable Amount IsTaxable cell
                var totalNonTaxAmtlastRightColumn = sheet.Cells[startIndex, 4];
                var totalNonTaxAmtlastRightColumnStyle = totalNonTaxAmtlastRightColumn.GetStyle();
                totalNonTaxAmtlastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtlastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtlastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                totalNonTaxAmtlastRightColumn.SetStyle(totalNonTaxAmtlastRightColumnStyle);

                startIndex++;
                #endregion

                #region Grand Total

                //Adding Total Taxable AmountLabel cell
                var grandTotal = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var grandTotalStyle = grandTotal.GetStyle();
                grandTotalStyle.Pattern = BackgroundType.Solid;
                grandTotalStyle.ForegroundColor = System.Drawing.Color.LightGray;
                grandTotalStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotal.SetStyle(grandTotalStyle);

                //Setting Total Taxable AmountLabel cell value
                grandTotal.Value = "Grand Total";

                //Adding Total Taxable Amount value cell
                var grandTotalValue = sheet.Cells[startIndex, 3];

                //Adding Total Taxable Amount value cell style
                var grandTotalValueStyle = grandTotalValue.GetStyle();
                grandTotalValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalValueStyle.Number = 2;
                grandTotalValue.SetStyle(grandTotalValueStyle);

                //Setting Total Taxable Amountcell value
                grandTotalValue.Value = report.GrandTotal;

                //Adding Total Taxable Amount IsTaxable cell
                var grandTotallastRightColumn = sheet.Cells[startIndex, 4];
                var grandTotallastRightColumnStyle = grandTotallastRightColumn.GetStyle();
                grandTotallastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotallastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotallastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotallastRightColumn.SetStyle(grandTotallastRightColumnStyle);

                startIndex++;
                startIndex++;
                
                #endregion

            }



            #endregion

            #region Footer

            //Adding Customer Address


            var messageLabel = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
            messageLabel.Merge();
            Style messageLabelStyle = workbook.CreateStyle();
            messageLabelStyle.Font.IsBold = true;
            messageLabelStyle.HorizontalAlignment = TextAlignmentType.Center;
            messageLabelStyle.VerticalAlignment = TextAlignmentType.Center;
            messageLabel.SetStyle(messageLabelStyle);
            messageLabel.Value = "THIS IS THE COMPUTER GENERATED PRINTOUT. NO SIGNATURE IS REQUIRED.";

            #endregion

            string fullSavePath = savePath + @"\" + Guid.NewGuid() + ".xlsx";

            workbook.Save(fullSavePath);

            return fullSavePath;
        }

        public string GenerateCreditNote(CreditNoteReportViewModel report, string excelTemplatePath, string savePath)
        {
            Workbook workbook = new Workbook(excelTemplatePath);

            Worksheet sheet = workbook.Worksheets[0];

            #region Credit Note Header

            //Adding Customer Name            
            Aspose.Cells.Cell customerName = sheet.Cells[2, 1];
            customerName.Value = report.Customer;

            //Adding Customer Address
            Aspose.Cells.Cell customerAddress = sheet.Cells[3, 1];
            customerAddress.Value = report.Address;

            //Adding Credit Note Number
            Aspose.Cells.Cell invoiceNo = sheet.Cells[2, 3];
            invoiceNo.Value = report.CNNo;

            //Adding Credit Note Date
            Aspose.Cells.Cell invoiceDate = sheet.Cells[4, 3];
            invoiceDate.Value = report.DateStr;

            #endregion

            #region Vessel Name

            //Adding Vessel Cell
            Aspose.Cells.Range vessel = sheet.Cells.CreateRange(7, 1, 1, 2);
            vessel.Merge();

            //Adding Cell Style

            ////Adding Border
            vessel.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);

            Style vesselStyle = new Style();

            ////Alignment
            vesselStyle.HorizontalAlignment = TextAlignmentType.Center;
            vesselStyle.VerticalAlignment = TextAlignmentType.Center;

            ////Font
            vesselStyle.Font.IsBold = true;
            vessel.SetStyle(vesselStyle);

            vessel.Value = "\"" + report.Vessel + "\"";

            #endregion

            #region Adding Credit Note Details

            int startIndex = 8;
            bool firstTime = true;

            foreach (var details in report.Details)
            {
                    //Adding blank row cell 
                    var mergeCell = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
                    mergeCell.Merge();

                    //Applying style to blank row cell

                    ////Adding Top Border to blank row if first invoice item
                    if (firstTime)
                    {
                        mergeCell.SetOutlineBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                        firstTime = false;
                    }

                    //Adding invoice detail description cell
                    var item = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
                    item.Merge();
                    item.Value = details.Description;
                    item.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    item.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);

                    //Adding invoice detail amount cell
                    var totalAmount = sheet.Cells[startIndex, 3];
                    var totalAmountStyle = totalAmount.GetStyle();
                    totalAmountStyle.Number = 2;
                    totalAmount.SetStyle(totalAmountStyle);
                    totalAmount.Value = details.TotalAmount;

                    //Adding invoice detail isTaxable cell
                    var rightColumn = sheet.Cells[startIndex, 4];
                    var rightColumnStyle = rightColumn.GetStyle();
                    rightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                    rightColumn.SetStyle(rightColumnStyle);
                    startIndex++;
                
            }

            //Adding Last Blank Row
            var lastBlockFirstColumn = sheet.Cells[startIndex, 0];
            var lastBlockFirstColumnStyle = lastBlockFirstColumn.GetStyle();
            lastBlockFirstColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockFirstColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockFirstColumn.SetStyle(lastBlockFirstColumnStyle);

            var lastBlockSecondColumn = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
            lastBlockSecondColumn.Merge();
            lastBlockSecondColumn.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockSecondColumn.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);

            var lastBlockThirdColumn = sheet.Cells.CreateRange(startIndex, 3, 1, 2);
            lastBlockThirdColumn.Merge();
            lastBlockThirdColumn.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastBlockThirdColumn.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);

            #endregion

            #region Grand Total

            startIndex++;
            startIndex++;

            //Adding Total Taxable Amount cell
            var totalTaxAmt = sheet.Cells[startIndex, 2];

            //Adding otal Taxable Amount Label Cell Style
            var totalTaxAmtStyle = totalTaxAmt.GetStyle();
            totalTaxAmtStyle.Pattern = BackgroundType.Solid;
            totalTaxAmtStyle.ForegroundColor = System.Drawing.Color.LightGray;
            totalTaxAmtStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            totalTaxAmtStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            totalTaxAmtStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            totalTaxAmtStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            totalTaxAmt.SetStyle(totalTaxAmtStyle);

            //Setting otal Taxable Amount Label cell value
            totalTaxAmt.Value = "Total Taxable Amount";

            //Adding otal Taxable Amountvalue cell
            var totalTaxAmtValue = sheet.Cells[startIndex, 3];

            //Adding otal Taxable Amount value cell style
            var totalTaxAmtValueStyle = totalTaxAmtValue.GetStyle();
            totalTaxAmtValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            totalTaxAmtValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            totalTaxAmtValueStyle.Number = 2;
            totalTaxAmtValue.SetStyle(totalTaxAmtValueStyle);

            //Setting otal Taxable Amount cell value
            totalTaxAmtValue.Value = report.TotalAmount;

            //Adding otal Taxable Amount IsTaxable cell
            var lastRightColumn = sheet.Cells[startIndex, 4];
            var lastRightColumnStyle = lastRightColumn.GetStyle();
            lastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
            lastRightColumn.SetStyle(lastRightColumnStyle);

            startIndex++;

            //Adding Grand Total Field

            if (report.IsTaxCN)
            {
                #region GST Amount

                //Adding Total Taxable AmountLabel cell
                var gst = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var gstStyle = gst.GetStyle();
                gstStyle.Pattern = BackgroundType.Solid;
                gstStyle.ForegroundColor = System.Drawing.Color.LightGray;
                gstStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gst.SetStyle(gstStyle);

                //Setting Total Taxable AmountLabel cell value
                gst.Value = "Add " + report.TaxDescription + " % GST on * " + report.TotalAmount;

                //Adding Total Taxable Amount value cell
                var gstValue = sheet.Cells[startIndex, 3];

                //Adding Total Taxable Amount value cell style
                var gstValueStyle = gstValue.GetStyle();
                gstValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstValueStyle.Number = 2;
                gstValue.SetStyle(gstValueStyle);

                //Setting Total Taxable Amountcell value
                gstValue.Value = report.TaxAmount;

                //Adding Total Taxable Amount IsTaxable cell
                var gstlastRightColumn = sheet.Cells[startIndex, 4];
                var gstlastRightColumnStyle = gstlastRightColumn.GetStyle();
                gstlastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstlastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstlastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                gstlastRightColumn.SetStyle(gstlastRightColumnStyle);

                startIndex++;

                #endregion
                
                #region Grand Total

                //Adding Total Taxable AmountLabel cell
                var grandTotal = sheet.Cells[startIndex, 2];

                //Adding Grand Total Label Cell Style
                var grandTotalStyle = grandTotal.GetStyle();
                grandTotalStyle.Pattern = BackgroundType.Solid;
                grandTotalStyle.ForegroundColor = System.Drawing.Color.LightGray;
                grandTotalStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalStyle.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotal.SetStyle(grandTotalStyle);

                //Setting Total Taxable AmountLabel cell value
                grandTotal.Value = "Total Non Taxable Amount";

                //Adding Total Taxable Amount value cell
                var grandTotalValue = sheet.Cells[startIndex, 3];

                //Adding Total Taxable Amount value cell style
                var grandTotalValueStyle = grandTotalValue.GetStyle();
                grandTotalValueStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalValueStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotalValueStyle.Number = 2;
                grandTotalValue.SetStyle(grandTotalValueStyle);

                //Setting Total Taxable Amountcell value
                grandTotalValue.Value = report.GrandTotal;

                //Adding Total Taxable Amount IsTaxable cell
                var grandTotallastRightColumn = sheet.Cells[startIndex, 4];
                var grandTotallastRightColumnStyle = grandTotallastRightColumn.GetStyle();
                grandTotallastRightColumnStyle.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotallastRightColumnStyle.SetBorder(BorderType.TopBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotallastRightColumnStyle.SetBorder(BorderType.RightBorder, CellBorderType.Thin, System.Drawing.Color.Black);
                grandTotallastRightColumn.SetStyle(grandTotallastRightColumnStyle);

                startIndex++;

                #endregion

            }

            startIndex++;


            #endregion

            #region Footer

            //Adding Customer Address


            var messageLabel = sheet.Cells.CreateRange(startIndex, 1, 1, 2);
            messageLabel.Merge();
            Style messageLabelStyle = workbook.CreateStyle();
            messageLabelStyle.Font.IsBold = true;
            messageLabelStyle.HorizontalAlignment = TextAlignmentType.Center;
            messageLabelStyle.VerticalAlignment = TextAlignmentType.Center;
            messageLabel.SetStyle(messageLabelStyle);
            messageLabel.Value = "THIS IS THE COMPUTER GENERATED PRINTOUT. NO SIGNATURE IS REQUIRED.";

            #endregion

            string fullSavePath = savePath + @"\" + Guid.NewGuid() + ".xlsx";

            workbook.Save(fullSavePath);

            return fullSavePath;
        }
    }

    public class CustomTitle
    {
        public int ColumnNo { get; set; }
        public string Value { get; set; }
    }
}
