using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.DTO
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    [Serializable]
    public class ImageResult
    {
        public bool Success { get; set; }
        public string ImageName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
