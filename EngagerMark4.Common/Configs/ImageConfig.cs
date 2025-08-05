using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Configs
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    /// <summary>
    /// Image configuration for folder location and image size
    /// Created by: Kyaw Min Htut
    /// Created date: 06-04-2015
    /// </summary>
    public class ImageConfig
    {
        public static string TYPE_FILE_LOCATION = "~/Images/Items/";

        public static string TYPE_MFILE_LOCATION = "~/Images/MItems/";

        public static string TYPE_RESOURCE_LOCATION = "~/Images/Resources/";

        public static string TYPE_MRESOURCE_LOCATION = "~/Images/MResources/";

        public static string TYPE_MDOC_LOCATION = "~/Csvs/";

        public static string TYPE_COMPANY_LOGO_LOCATION = "~/Images/Company/";

        public static string SIGNATURE_LOCATION = "\\Images\\Items\\";

        public static int WIDTH = 600;

        public static int MWIDTH = 90;

        public static string TYPE_SIGNATURE_LOCATION = "~/Images/Signatures/";
    }
}
