using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Application
{
    [Serializable]
    [Table("Tb_SMTP",Schema = "Core")]
    public class SMTP : BasicEntity
    {
        [Display(Name = "SMTP Server")]
        [Required]
        [StringLength(255)]
        public string SMTPServer
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Credential
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Password
        {
            get;
            set;
        }

        [NotMapped]
        public string PasswordStr
        {
            get
            {
                if (string.IsNullOrEmpty(Password)) return "";
                return Cryptography.Decrypt(Password, true);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    Password = Cryptography.Encrypt(value == null ? "" : value, true);
            }
        }

        public bool SSL
        {
            get;
            set;
        }

        [Display(Name = "Admin Mail")]
        [StringLength(255)]
        [EmailAddress]
        public string AdminMail
        {
            get;
            set;
        }

        [Display(Name = "Support Mail")]
        [StringLength(255)]
        [EmailAddress]
        public string SupportMail
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(SMTP) + " : " + SMTPServer;
        }
    }
}
