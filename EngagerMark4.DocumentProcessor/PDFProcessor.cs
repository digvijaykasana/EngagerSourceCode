using Aspose.Cells;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Slides;
using Aspose.Words;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Common.Utility;
using System;
using System.IO;

namespace EngagerMark4.DocumentProcessor
{
    public class PDFProcessor
    {
        public string GeneratePDF(string htmlString, string savePath, string poId)
        {
            Aspose.Pdf.HtmlLoadOptions htmlLoadOptions = new Aspose.Pdf.HtmlLoadOptions();
            htmlLoadOptions.PageInfo.Margin.Bottom = 5;
            htmlLoadOptions.PageInfo.Margin.Top = 15;
            htmlLoadOptions.PageInfo.Margin.Left = 30;
            htmlLoadOptions.PageInfo.Margin.Right = 30;

            // Create a directory if not existed.
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // Load HTML file
            using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlString)), htmlLoadOptions))
            {

                //Adding Page Number for each page
                doc.ProcessParagraphs();
                TextStamp textStamp = new TextStamp("");
                textStamp.HorizontalAlignment = HorizontalAlignment.Right;
                textStamp.VerticalAlignment = VerticalAlignment.Bottom;

                textStamp.RightMargin = 30;
                textStamp.BottomMargin = 5;

                foreach (Page page in doc.Pages)
                {
                    textStamp.Value = $"Page{page.Number} / {doc.Pages.Count}";
                    page.AddStamp(textStamp);
                }

                DateTime time = TimeUtil.GetLocalTime();
                string l_time = time.Day.ToString() + "" + time.Hour.ToString() + "" + time.Minute.ToString() + "" + time.Second.ToString() + "" + time.Millisecond.ToString();
                // Save HTML file
                string fullFilePath = savePath + TextManager.FilterTextandNumber(poId) + "-v" + l_time + ".pdf";
                if (File.Exists(fullFilePath))
                {
                    try
                    {
                        File.Delete(fullFilePath);

                    }
                    catch (Exception ex)
                    {

                    }
                }

                try
                {
                    doc.Save(fullFilePath);
                }
                catch
                {

                }
                return fullFilePath;
            }
        }

        public void ConcatenatePDF(string[] inputFiles, ref string outputFile)
        {
            PdfFileEditor pdfEditor = new PdfFileEditor();
            //pdfEditor.Concatenate(inputFiles, outputFile);
        }

        private Stream FormatPageSize(Stream fileStream, PageSize pageSize)
        {
            //if (!File.Exists(filePath))
            //    return;

            PdfPageEditor pEdit = new PdfPageEditor();

            pEdit.BindPdf(fileStream);

            int[] pages = new int[pEdit.GetPages()];

            for (int i = 1; i <= pages.Length; i++)
            {
                int index = i - 1;
                pages[index] = i;
            }

            pEdit.ProcessPages = pages;
            pEdit.PageSize = pageSize;
            pEdit.Document.FitWindow = true;

            Stream outPutStream = new MemoryStream();
            pEdit.Save(outPutStream);
            pEdit = null;
            return outPutStream;
            //if (File.Exists(filePath))
            //    File.Delete(filePath);
            //pEdit.Save(filePath);


        }

        private void ConvertFromWord(Stream inputStream, Stream outputStream)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(inputStream);
            int pageCount = doc.PageCount;
            NodeCollection paragraphs = doc.GetChildNodes(NodeType.Paragraph, true);

            foreach (Aspose.Words.Paragraph para in paragraphs)
            {
                // If the paragraph has a page break before set then clear it
                if (para.ParagraphFormat.PageBreakBefore)
                    para.ParagraphFormat.PageBreakBefore = false;

                // Check all runs in the paragraph for page breaks and remove them.
                foreach (Run run in para.Runs)
                {
                    if (run.Text.Contains(ControlChar.PageBreak))
                        run.Text = run.Text.Replace(ControlChar.PageBreak, string.Empty);
                }
            }
            //double pageHeight = 0;
            //double pageWidth = 0;
            //double pageTopMargin = 0;
            //double pageBottomMargin = 0;
            //double pageLeftMargin = 0;
            //double pageRightMargin = 0;
            //int count = doc.Sections.Count;
            //foreach (Section section in doc.Sections)
            //{
            //    pageHeight = section.PageSetup.PageHeight;
            //    pageWidth = section.PageSetup.PageWidth;
            //    pageTopMargin = section.PageSetup.TopMargin;
            //    pageBottomMargin = section.PageSetup.BottomMargin;
            //    pageLeftMargin = section.PageSetup.LeftMargin;
            //    pageRightMargin = section.PageSetup.RightMargin;
            //}
            ////doc.Range.Bookmarks[0].Text = string.Empty;
            ////doc.Range.Bookmarks[0].BookmarkStart.ParentNode.Remove();
            //DocumentBuilder docBuilder = new DocumentBuilder(doc);
            //docBuilder.PageSetup.PageHeight = pageHeight;
            //docBuilder.PageSetup.PageWidth = pageWidth;
            ////docBuilder.PageSetup.TopMargin = pageTopMargin;
            //docBuilder.PageSetup.TopMargin = pageTopMargin;
            //docBuilder.PageSetup.BottomMargin = pageBottomMargin;
            //docBuilder.PageSetup.LeftMargin = pageLeftMargin;
            //docBuilder.PageSetup.RightMargin = pageRightMargin;
            //docBuilder.PageSetup.
            //ParagraphFormat paragraphFormat = docBuilder.ParagraphFormat;
            //paragraphFormat.Alignment = ParagraphAlignment.Center;
            //paragraphFormat.LeftIndent = 0;
            //paragraphFormat.RightIndent = 0;
            //paragraphFormat.SpaceAfter = 0;
            //paragraphFormat.NoSpaceBetweenParagraphsOfSameStyle = true;
            //paragraphFormat.PageBreakBefore = false;
            //paragraphFormat.SpaceBefore = 0;
            //paragraphFormat.KeepTogether = true;
            //docBuilder.Document.Save(outputStream, Aspose.Words.SaveFormat.Pdf);
            doc.Save(outputStream, Aspose.Words.SaveFormat.Pdf);
            doc = null;
        }

        public MemoryStream ConvertPDF(Stream inputStream, string fileName)
        {
            string fileExtension = System.IO.Path.GetExtension(fileName);

            MemoryStream outputStream = new MemoryStream();

            switch (fileExtension)
            {
                case ".doc":
                case ".docx":
                    Aspose.Words.Document doc = new Aspose.Words.Document(inputStream);
                    doc.Save(outputStream, Aspose.Words.SaveFormat.Pdf);
                    doc = null;
                    //ConvertFromWord(inputStream, outputStream);
                    break;
                case ".xls":
                case ".xlsx":
                case ".xlsm":
                case ".xlt":
                    Workbook book = new Workbook(inputStream);
                    foreach (var ws in book.Worksheets)
                    {
                        ws.PageSetup.FitToPagesWide = 1;
                        ws.PageSetup.PaperSize = PaperSizeType.PaperA4;
                    }
                    Aspose.Cells.PdfSaveOptions saveOptions = new Aspose.Cells.PdfSaveOptions(Aspose.Cells.SaveFormat.Pdf);
                    book.Save(outputStream, saveOptions);
                    //book.Save(outputStream, pdfSaveOptions);
                    //Aspose.Cells.PdfSaveOptions pdfSaveOptions = new Aspose.Cells.PdfSaveOptions();
                    //pdfSaveOptions.OnePagePerSheet = true;
                    //pdfSaveOptions.AllColumnsInOnePagePerSheet = true;
                    //pdfSaveOptions.Compliance = Aspose.Cells.Rendering.PdfCompliance.PdfA1b;
                    ////book.Save(outputStream, Aspose.Cells.SaveFormat.Pdf);
                    //book.Save(outputStream, pdfSaveOptions);
                    //outputStream = (MemoryStream)FormatPageSize(outputStream, PageSize.A4);
                    book = null;
                    break;
                case "ppt":
                case "pptx":
                    Presentation pres = new Presentation(inputStream);
                    pres.Save(outputStream, Aspose.Slides.Export.SaveFormat.Pdf);
                    pres = null;
                    break;
                default:
                    break;
            }

            return outputStream;
        }

        public string ConvertToExcel(string inputFile, string outputFolder, bool isxlsx = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string outputFile = outputFolder + KeyUtil.GenerateKey() + (isxlsx == true ? ".xlsx" : ".xls");

            // Load PDF document
            using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputFile))
            {
                // Instantiate ExcelSave Option object
                Aspose.Pdf.ExcelSaveOptions excelsave = new Aspose.Pdf.ExcelSaveOptions();
                excelsave.MinimizeTheNumberOfWorksheets = true;
                // Save the output in XLS format
                pdfDocument.Save(outputFile, excelsave);

                return outputFile;
            }
        }

        public int ImageLowerLeftX
        { get; set; }

        public int ImageLowerLeftY
        { get; set; }

        public int ImageUpperRightX
        { get; set; }

        public int ImageUpperRightY
        { get; set; }

        public PDFProcessor()
        {
            this.ImageLowerLeftX = 150;
            this.ImageLowerLeftY = 92;
            this.ImageUpperRightX = 245;
            this.ImageUpperRightY = 107;
        }

        public void InsertImageInPDF(Stream pdfInputStream, ref MemoryStream pdfOutputStream, Stream imageStream, int pageNumber)
        {
            PdfFileMend objFileMend = new PdfFileMend(pdfInputStream, pdfOutputStream);
            objFileMend.AddImage(imageStream, pageNumber, ImageLowerLeftX, ImageLowerLeftY, ImageUpperRightX, ImageUpperRightY);
            objFileMend.Close();
        }

        public void RemovePages(Stream pdfInputStream, ref MemoryStream pdfOutPutStream, int[] pages)
        {
            using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document(pdfInputStream))
            {
                foreach (int i in pages)
                {
                    try
                    {
                        doc.Pages.Delete(i);
                    }
                    catch
                    {

                    }
                }
                doc.Save(pdfOutPutStream);
            }
        }
    }
}
