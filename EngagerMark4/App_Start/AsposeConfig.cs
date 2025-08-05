using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EngagerMark4.App_Start
{
    public class AsposeConfig
    {
        public static void RegisterLicense(HttpServerUtility Server)
        {
            
            //string testPath = Server.MapPath(FileConfig.ASPOSE_TOTAL_LICENSE);
            
            AsposeLicense.WORD_LICENSE = new Aspose.Words.License();
            AsposeLicense.WORD_LICENSE.SetLicense(Server.MapPath(FileConfig.ASPOSE_TOTAL_LICENSE));
            AsposeLicense.CELL_LICENSE = new Aspose.Cells.License();
            AsposeLicense.CELL_LICENSE.SetLicense(Server.MapPath(FileConfig.ASPOSE_TOTAL_LICENSE));
            AsposeLicense.PDF_LICENSE = new Aspose.Pdf.License();
            AsposeLicense.PDF_LICENSE.SetLicense(Server.MapPath(FileConfig.ASPOSE_PDF_LICENSE));
        }
    }
}